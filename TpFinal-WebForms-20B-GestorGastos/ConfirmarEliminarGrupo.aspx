﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ConfirmarEliminarGrupo.aspx.cs" Inherits="TpFinal_WebForms_20B_GestorGastos.ConfirmarEliminar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .success-container {
            margin-top: 50px;
            text-align: center;
        }

        .success-icon {
            font-size: 100px;
            color: #28a745;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container success-container">
        <img src="/Images/logoWarning.png" alt="Warning" class="success-image" style="width: 100px; height: 100px;" />
        <h5>¿Está seguro que desea eliminar el grupo con todos sus participantes?</h5>
        <asp:Button ID="btnConfirmar" Text="Confirmar" CssClass="btn btn-secondary" runat="server" OnClick="btnConfirmar_Click" />
        <asp:Button ID="btnCancelar" Text="Cancelar" CssClass="btn btn-secondary" runat="server" OnClick="btnCancelar_Click" />
    </div>
</asp:Content>
