<%@ Page Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="actualizacion_facturas.aspx.vb" Inherits="ops_pages_actualizacion_facturas" Title="Actualizacion Facturas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <link rel="stylesheet" type="text/css" media="screen" href="../css/jquery-ui-1.8.2.custom.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../css/ui.jqgrid.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../css/ui.multiselect.css" />
    <link rel="Stylesheet" type="text/css" media="screen" href="../css/jquery.fileupload.css" />
    <link rel="Stylesheet" type="text/css" media="screen" href="../css/jquery.fileupload.ui.css" />
    <link rel="Stylesheet" type="text/css" media="screen" href="../css/Punto_Venta2.css" />
    <link type="text/css" href="../Skin/CSS/fontawesome.min.css" rel="stylesheet" />
    <style>
        .error {
            color: red;
            display: inline;
            font-style: italic;
            font-size: small;
            font-weight: lighter;
            position: relative;
        }

        .row-bottom-margin {
            margin-bottom: 5px;
        }

        .form-group {
            margin-bottom: 7px;
        }

        .panel-body {
            padding: 7px;
        }

        .control-label {
            padding-top: 7px;
            margin-bottom: 0;
            text-align: right;
            font-size: 85%;
        }
        .ui-jqdialog-content .CaptionTD {
            width: 60% !important;
        }
        .ui-jqgrid .ui-jqgrid-htable th div
        {
            height: auto;
            overflow: hidden;
            padding-right: 4px;
            padding-top: 2px;            
            vertical-align: text-top;
            white-space: normal !important;
        }
        .gridWidth {
            overflow: auto !important;
            width: 1100px !important;
        }
    </style>
    <div id="page-wrapper" style="width: 700px; margin-right: 15px; position: absolute; top: 145px; height: 175px;">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-12">
                    <fieldset>
                        <!-- Form Name -->
                        <legend>Actualizacion Facturas</legend>
                        <div class="row">
                            
                        </div>                        
                        <div class="row">
                            <div class="col-lg-4">
                                <span class="btn btn-warning btn-xs fileinput-button">
                                    <i class="glyphicon glyphicon-plus"></i>
                                    <span>Agregar Archivo...</span>
                                    <!-- The file input field used as target for the file upload widget -->
                                    <input id="fileupload" type="file" name="files[]" multiple>
                                </span>
                            </div>
                            <div class="col-lg-4">
                                <div id="files" class="files"></div>
                            </div>
                        </div>                        
                        <div class="row">
                            <div class="col-lg-12">
                                <label class="col-lg-4 control-label" for="datepicker">Nombre Archivo:</label>
                                <div class="col-lg-8">
                                    <h6>
                                        <label id="attachedfiles" class="label label-success"></label>
                                    </h6>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <input class="btn btn-success" type="button" id="btnUploadTarifas" name="btnUploadTarifas" value="Actualizar" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-xs-12 col-sm-12 text-center gridWidth" id="estafetaRequest">
                                <table id="gridFacturas" class=" table table-striped table-bordered table-condensed"></table>
                                <div id="pagerFacturas"></div>
                            </div>                            
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>    
</asp:Content>
<asp:Content ContentPlaceHolderID="ScriptSection" runat="server">
    <script type="text/javascript" src="../Scripts/grid.locale-en.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.jqGrid.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.validator.js"></script>
    <script type="text/javascript" src="../Scripts/actualizacionFacturas.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.iframe-transport.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.fileupload.js"></script>    
    <script type="text/javascript" src="../Scripts/FileUploadActualizacionFacturas.js"></script>    
</asp:Content>
