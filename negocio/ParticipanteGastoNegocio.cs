﻿using dominio;
using System;
using System.Collections.Generic;

namespace negocio
{
    public class ParticipanteGastoNegocio
    {
        public void AgregarParticipante(ParticipanteGasto participante)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO ParticipantesGasto (idGasto, idUsuario, montoIndividual) VALUES (@idGasto, @idUsuario, @montoIndividual)");
                datos.setearParametro("@idGasto", participante.IdGasto);
                datos.setearParametro("@idUsuario", participante.IdUsuario);
                datos.setearParametro("@montoIndividual", participante.MontoIndividual);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public bool AgregarParticipanteAGrupo(int idGrupo, int idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Verificar si el usuario existe en la tabla Usuarios
                datos.setearConsulta("SELECT COUNT(*) FROM Usuarios WHERE idUsuario = @idUsuario AND Activo = 1");
                datos.setearParametro("@idUsuario", idUsuario);
                int usuarioExiste = (int)datos.ejecutarScalar();

                if (usuarioExiste == 0)
                {
                    return false;
                }

                // Verificar si el usuario ya es miembro del grupo
                datos.setearConsulta("SELECT COUNT(*) FROM MiembrosGrupos WHERE idGrupo = @idGrupo AND idUsuario = @idUsuario");
                datos.setearParametro("@idGrupo", idGrupo);
                datos.setearParametro("@idUsuario", idUsuario);
                int miembroExiste = (int)datos.ejecutarScalar();

                if (miembroExiste > 0)
                {
                    return false;
                }

                // Insertar al usuario en el grupo
                datos.setearConsulta("INSERT INTO MiembrosGrupos (idGrupo, idUsuario, fechaUnion, rol) VALUES (@idGrupo, @idUsuario, @fechaUnion, @rol)");
                datos.setearParametro("@idGrupo", idGrupo);
                datos.setearParametro("@idUsuario", idUsuario);
                datos.setearParametro("@fechaUnion", DateTime.Now);
                datos.setearParametro("@rol", "miembro");
                datos.ejecutarAccion();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public List<ParticipanteGasto> listarParticipantesConEstadoPago(int idGasto)
        {
            AccesoDatos datos = new AccesoDatos();
            List<ParticipanteGasto> lista = new List<ParticipanteGasto>();
            try
            {
                datos.setearConsulta(@"
                SELECT 
                    pg.idUsuario, 
                    u.Nombre AS NombreUsuario, 
                    u.Email AS EmailUsuario,
                    pg.montoIndividual, 
                    ISNULL(SUM(p.montoPagado),0) AS montoPagado
                FROM ParticipantesGasto pg
                LEFT JOIN Usuarios u ON pg.idUsuario = u.idUsuario
                LEFT JOIN Pagos p ON pg.idGasto = p.idGasto AND pg.idUsuario = p.idUsuario
                WHERE pg.idGasto = @idGasto
                GROUP BY pg.idUsuario, u.Nombre, u.Email, pg.montoIndividual");
                datos.setearParametro("@idGasto", idGasto);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    ParticipanteGasto participante = new ParticipanteGasto
                    {
                        IdUsuario = (int)datos.Lector["idUsuario"],
                        NombreUsuario = datos.Lector["NombreUsuario"].ToString(),
                        EmailUsuario = datos.Lector["EmailUsuario"].ToString(),
                        MontoIndividual = (decimal)datos.Lector["montoIndividual"],
                        MontoPagado = (decimal)datos.Lector["montoPagado"]
                    };

                    participante.DeudaPendiente = participante.MontoIndividual - participante.MontoPagado;

                    lista.Add(participante);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar participantes con estado de pago", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public decimal obtenerMontoPagadoPorUsuario(int idGasto, int idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT ISNULL(SUM(montoPagado),0) FROM Pagos WHERE idGasto = @idGasto AND idUsuario = @idUsuario");
                datos.setearParametro("@idGasto", idGasto);
                datos.setearParametro("@idUsuario", idUsuario);

                object resultado = datos.ejecutarScalar();
                if (resultado == null || resultado == DBNull.Value)
                {
                    return 0;
                }
                else
                {

                    return Convert.ToDecimal(resultado);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public Usuario obtenerUsuarioCreadorPorIdGasto(int idGasto)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {                
                datos.setearConsulta(@"
                SELECT u.idUsuario, u.Nombre, u.Email 
                FROM Gastos g
                INNER JOIN Usuarios u ON g.creadoPor = u.idUsuario
                WHERE g.idGasto = @idGasto
                ");
                datos.setearParametro("@idGasto", idGasto);

                datos.ejecutarLectura();
                
                if (datos.Lector.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        IdUsuario = (int)datos.Lector["idUsuario"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Email = datos.Lector["Email"].ToString()
                    };
                    return usuario;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario creador por idGasto", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}

