<%@ Page Title="Session Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SessionPage.aspx.cs" Inherits="WebFormsApp.SessionPage" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderPlaceHolder" runat="server">
    <base href="/" />
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>

        <div>
            <p>Session Message:<asp:TextBox ID="SessionTextBox" runat="server" /></p>
            <asp:Button ID="SubmitSessionButton" runat="server" Text="Submit" OnClick="OnSubmitSessionButtonClick" />
        </div>

        <div>
            <hello-world path="<%: Context.Request.Path %>"></hello-world>
        </div>
    </main>
</asp:Content>

<asp:Content ID="ScriptsContent" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    <script src="_framework/blazor.server.js"></script>
</asp:Content>