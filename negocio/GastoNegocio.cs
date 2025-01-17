﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using dominio;

namespace negocio
{
    public class GastoNegocio
    {
        public List<Gasto> listar()
        {
            List<Gasto> lista = new List<Gasto>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IdGasto, FechaGasto, Descripcion, MontoTotal FROM Gastos");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Gasto aux = new Gasto();
                    aux.IdGasto = (int)datos.Lector["IdGasto"];
                    aux.FechaGasto = (DateTime)datos.Lector["FechaGasto"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.MontoTotal = (decimal)datos.Lector["MontoTotal"];

                    lista.Add(aux);
                }

                return lista;
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

        public int AgregarGasto(Gasto nuevoGasto)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO Gastos (IdGrupo, Descripcion, MontoTotal, FechaGasto, CreadoPor) OUTPUT INSERTED.IdGasto VALUES (@IdGrupo, @Descripcion, @MontoTotal, @FechaGasto, @CreadoPor)");

                datos.setearParametro("@IdGrupo", nuevoGasto.IdGrupo);
                datos.setearParametro("@Descripcion", nuevoGasto.Descripcion);
                datos.setearParametro("@MontoTotal", nuevoGasto.MontoTotal);
                datos.setearParametro("@FechaGasto", nuevoGasto.FechaGasto);
                datos.setearParametro("@CreadoPor", nuevoGasto.CreadoPor);

                int idGastoAutoGenerado = (int)datos.ejecutarScalar();

                return idGastoAutoGenerado;
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

        public void eliminarGasto(int idGasto)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM ParticipantesGasto Where idGasto = @idGastoParticipantesGasto");
                datos.setearParametro("@idGastoParticipantesGasto", idGasto);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                datos.setearConsulta("DELETE FROM Gastos Where idGasto = @idGasto");
                datos.setearParametro("@idGasto", idGasto);

                datos.ejecutarAccion();
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

        public void modificar(Gasto gasto)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE Gastos SET descripcion = @descripcion, montoTotal = @monto, fechaGasto = @fecha WHERE idGasto = @id");
                datos.setearParametro("@descripcion", gasto.Descripcion);
                datos.setearParametro("@monto", gasto.MontoTotal);
                datos.setearParametro("@fecha", gasto.FechaGasto);
                datos.setearParametro("@id", gasto.IdGasto);
                
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string ObtenerNombreUsuario(int idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT nombre FROM Usuarios WHERE idUsuario = @idUsuario");
                datos.setearParametro("@idUsuario", idUsuario);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    return (string)datos.Lector["nombre"];
                }
                return string.Empty;
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

        public string ObtenerEmailUsuario(int idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT email FROM Usuarios WHERE idUsuario = @idUsuario");
                datos.setearParametro("@idUsuario", idUsuario);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    return (string)datos.Lector["email"];
                }
                return string.Empty;
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

        public List<ParticipanteGasto> ListarParticipantesPorGrupo(int idGrupo)
        {
            List<ParticipanteGasto> participantesGasto = new List<ParticipanteGasto>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            SELECT mg.idUsuario 
            FROM MiembrosGrupos mg 
            WHERE mg.idGrupo = @idGrupo");
                datos.setearParametro("@idGrupo", idGrupo);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    ParticipanteGasto participante = new ParticipanteGasto
                    {
                        IdUsuario = (int)datos.Lector["idUsuario"]
                    };

                    participantesGasto.Add(participante);
                }

                return participantesGasto;
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

        public List<Grupo> ListarGrupos()
        {
            List<Grupo> listaGrupos = new List<Grupo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IdGrupo, NombreGrupo FROM Grupos");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Grupo grupo = new Grupo
                    {
                        IdGrupo = (int)datos.Lector["IdGrupo"],
                        NombreGrupo = (string)datos.Lector["NombreGrupo"]
                    };
                    listaGrupos.Add(grupo);
                }

                return listaGrupos;
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
        public List<Grupo> listarGrupoModificar(int id)
        {
            List<Grupo> listaGrupos = new List<Grupo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IdGrupo, NombreGrupo FROM Grupos WHERE IdGrupo = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Grupo grupo = new Grupo
                    {
                        IdGrupo = (int)datos.Lector["IdGrupo"],
                        NombreGrupo = (string)datos.Lector["NombreGrupo"]
                    };
                    listaGrupos.Add(grupo);
                }

                return listaGrupos;
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

        public int? ObtenerIdUsuarioPorEmail(string email)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT idUsuario FROM Usuarios WHERE email = @Email AND Activo = 1");
                datos.setearParametro("@Email", email);
                object resultado = datos.ejecutarScalar();

                if (resultado != null)
                    return (int)resultado;
                else
                    return null;
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

        public bool AgregarParticipanteAGrupo(int idGrupo, int idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
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

        public bool EliminarParticipanteDeGrupo(int idGrupo, int idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM MiembrosGrupos WHERE idGrupo = @idGrupo AND idUsuario = @idUsuario");
                datos.setearParametro("@idGrupo", idGrupo);
                datos.setearParametro("@idUsuario", idUsuario);
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

     
        public List<Gasto> listarGastosPorUsuario(int idUsuario)
        {
            List<Gasto> lista = new List<Gasto>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT g.IdGasto,g.IdGrupo, g.FechaGasto, g.Descripcion, g.MontoTotal FROM Gastos g JOIN MiembrosGrupos mg ON g.IdGrupo = mg.IdGrupo WHERE mg.IdUsuario = @idUsuario");

                datos.setearParametro("@idUsuario", idUsuario);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Gasto aux = new Gasto();
                    aux.IdGasto = (int)datos.Lector["IdGasto"];
                    aux.FechaGasto = (DateTime)datos.Lector["FechaGasto"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.MontoTotal = (decimal)datos.Lector["MontoTotal"];
                    aux.IdGrupo = (int)datos.Lector["IdGrupo"];
                    lista.Add(aux);
                }

                return lista;
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
        public List<Grupo> listarGruposPorUsuario(int idUsuario)
        {
            List<Grupo> lista = new List<Grupo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT g.IdGrupo, g.NombreGrupo FROM Grupos g JOIN MiembrosGrupos mg ON g.IdGrupo = mg.IdGrupo WHERE mg.idUsuario = @idUsuario");
                datos.setearParametro("@idUsuario", idUsuario);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Grupo aux = new Grupo();
                    aux.IdGrupo = (int)datos.Lector["IdGrupo"];
                    aux.NombreGrupo = (string)datos.Lector["NombreGrupo"];
                    lista.Add(aux);
                }

                return lista;
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

        public List<Gasto> listarPorId(string id)
        {
            List<Gasto> lista = new List<Gasto>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IdGasto, idGrupo, Descripcion, MontoTotal, fechaGasto FROM Gastos WHERE IdGasto = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Gasto aux = new Gasto();
                    aux.IdGasto = (int)datos.Lector["IdGasto"];
                    aux.IdGrupo = (int)datos.Lector["idGrupo"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.MontoTotal = (decimal)datos.Lector["MontoTotal"];
                    aux.FechaGasto = (DateTime)datos.Lector["FechaGasto"];

                    lista.Add(aux);
                }
                return lista;
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
               

        public Gasto obtenerGastoPorId(int idGasto)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT IdGasto, IdGrupo, Descripcion, MontoTotal, FechaGasto, CreadoPor FROM Gastos WHERE IdGasto = @idGasto");
                datos.setearParametro("@idGasto", idGasto);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Gasto gasto = new Gasto
                    {
                        IdGasto = (int)datos.Lector["IdGasto"],
                        IdGrupo = (int)datos.Lector["IdGrupo"],
                        Descripcion = (string)datos.Lector["Descripcion"],
                        MontoTotal = (decimal)datos.Lector["MontoTotal"],
                        FechaGasto = (DateTime)datos.Lector["FechaGasto"],
                        CreadoPor = (int)datos.Lector["CreadoPor"]
                    };
                    return gasto;
                }
                return null;
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
        public List<Gasto> listarGastosPorGrupo(int idGrupo)
        {
            List<Gasto> lista = new List<Gasto>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT idGasto, idGrupo, descripcion, montoTotal, fechaGasto, creadoPor FROM Gastos WHERE idGrupo = @idGrupo");
                datos.setearParametro("@idGrupo", idGrupo);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Gasto aux = new Gasto();
                    aux.IdGasto = (int)datos.Lector["idGasto"];
                    aux.IdGrupo = (int)datos.Lector["idGrupo"];
                    aux.Descripcion = (string)datos.Lector["descripcion"];
                    aux.MontoTotal = (decimal)datos.Lector["montoTotal"];
                    aux.FechaGasto = (DateTime)datos.Lector["fechaGasto"];
                    aux.CreadoPor = (int)datos.Lector["creadoPor"];

                    lista.Add(aux);
                }

                return lista;
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
    }
}
