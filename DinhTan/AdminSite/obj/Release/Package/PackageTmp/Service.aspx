﻿<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Service.aspx.cs" Inherits="AdminSite.Service" %>

<%@ Register Src="~/UserControls/ucMenuDetail.ascx" TagPrefix="uc1" TagName="ucMenuDetail" %>
<%@ Register Src="~/UserControls/ucNewsItems.ascx" TagPrefix="uc1" TagName="ucNewsItems" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <uc1:ucMenuDetail runat="server" ID="ucMenuDetail" />
    <uc1:ucNewsItems runat="server" ID="ucNewsItems" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
