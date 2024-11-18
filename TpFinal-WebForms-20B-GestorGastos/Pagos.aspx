﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Pagos.aspx.cs" Inherits="TpFinal_WebForms_20B_GestorGastos.Pagos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <h2>Pagos</h2>
        <div class="mb-3">
            <label for="ddlGrupos" class="form-label">Selecciona un Grupo</label>
            <asp:DropDownList ID="ddlGrupos" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlGrupos_SelectedIndexChanged">
            </asp:DropDownList>
        </div>

        <div id="gastosContainer" runat="server" visible="false">
            <h3>Lista de Gastos</h3>
            <asp:Repeater ID="repGastos" runat="server" OnItemCommand="repGastos_ItemCommand">
                <ItemTemplate>
                    <div class="card mb-3">
                        <div class="card-body">
                            <h5 class="card-title"><%# Eval("Descripcion") %></h5>
                            <p class="card-text">Monto: <%# Eval("MontoTotal") %></p>
                            <p class="card-text">Fecha: <%# Eval("FechaGasto") %></p>
                            <asp:Button ID="btnSeleccionarGasto" runat="server" CommandName="SeleccionarGasto" CommandArgument='<%# Eval("IdGasto") %>' Text="Seleccionar" CssClass="btn btn-secondary" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>


        <h2 class="text-center">Detalle del Gasto</h2>


        <div id="detalleGastoContainer" runat="server" visible="false" class="mt-4">
            <h3>Detalle del Gasto</h3>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>Descripción</th>
                        <th>Fecha</th>
                        <th>Monto Total</th>
                        <th>Grupo</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <asp:Label ID="lblDescripcion" runat="server" /></td>
                        <td>
                            <asp:Label ID="lblFechaGasto" runat="server" /></td>
                        <td>
                            <asp:Label ID="lblMontoTotal" runat="server" /></td>
                        <td>
                            <asp:Label ID="lblGrupo" runat="server" /></td>
                    </tr>
                </tbody>
            </table>
        </div>


        <!-- tabla de participantes. vamos a llamar al mismo repeater -->
        <h3 class="text-center">Participantes y Pagos</h3>
        <div class="card">
            <div class="card-body">
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th>Participante</th>
                            <th>Email</th>
                            <th>Monto Individual</th>
                            <th>Monto Pagado</th>
                            <th>Deuda Pendiente</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repPagosParticipantes" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("IdUsuario") %></td>
                                    <td>---</td> <!-- aca hay que agregar la logica para nombre de usuario y el email-->
                                    <td>$<%# Eval("MontoIndividual") %></td>
                                    <td>$<%# Eval("MontoPagado") %></td>
                                    <td>$<%# Eval("DeudaPendiente") %></td>
                                </tr>
                                <!-- hacer el eval de deuda pendiente-->
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="text-center mt-4">
            <asp:Button ID="btnConfirmarPagos" runat="server" CssClass="btn btn-secondary" Text="Confirmar Pagos" />
        </div>
    </div>



</asp:Content>
