/* This is a template for page-related javascript */
var IMPORTARRECONCILIACIONPROVEEDORES = (function ($) { // parameters relate to scope
    /*  Hoisting Private variables */
    var gridColumns = [
        "Proveedor", "Guia Proveedor", "Guia Draft", "Costo", "Peso", "No. Cajas"
    ];

    var gridColumnsModel = [
        { name: "Proveedor", index: "Proveedor", width: 100, align: "Left", sorttype: "int", hidden: true },
        { name: "guia_proveedor", index: "guia_proveedor", width: 100, align: "Left" },
        { name: "guia_draft", index: "guia_draft", width: 100, align: "Left" },
        { name: "costo", index: "costo", width: 100, align: "Left" },
        { name: "peso", index: "peso", width: 100, align: "Left" },
        { name: "no_cajas", index: "no_cajas", width: 100, align: "Left" },       
    ];    

    function insertarConciliaciones() {
        var rows = jQuery("#gridProveedores").jqGrid('getRowData');        
        if (rows.length > 0) {            
            var url = "importar_reconciliacion_proveedores.aspx/InsertarReconciliacion";
            var params = '{conciliacion:' + JSON.stringify(rows) + '}';
            var errMsg = "Error al importar datos: ";

            var response = genericCallWebMethod(url, params, errMsg, false);
            if (response && response.responseMessage) {
                alert(response.responseMessage);
            }
        } else {
            alert('No hay registros para importar');
        }        
    }    
    
    function createEmptyGrid() {
        var gridName = "#gridProveedores";
        var pagerName = "#pagerProveedores";
        var sortName = "Proveedor";
        var sortOrder = "asc";
        
        createGrid(gridName, pagerName, "", gridColumns, gridColumnsModel, sortName, sortOrder, "1100", "450");
    }    

    function genericCallWebMethod(urlParam, params, errMsg, paramAsync) {
        var resp;

        $.ajax({
            type: "POST",
            url: urlParam,
            data: params,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log(response)
                resp = response.d;
                if (response.d.responseSuccess == 0) {
                    alert(response.d.responseMessage);
                }
            },
            async: paramAsync,
            error: function (xhr, errorType, exception) { //Triggered if an error communicating with server  
                console.log(xhr, exception)
                var errorMessage = exception || xhr.statusText; //If exception null, then default to xhr.statusText           
                alert(errMsg + errorMessage);
            }
        });

        return resp;
    }

    function createGrid(gridName, pagerName, myData, gridColNames, gridColModel, sortName, sortOrder, width, height, multiselect, rownumbers, rowNum) {
        if (width == null) {
            width = '600';
        }
        if (height == null) {
            height = '1020';
        }
        if (multiselect == null) {
            multiselect = false;
        }
        if (rownumbers == null) {
            rownumbers = true;
        }
        if (rowNum == null) {
            rowNum = 100;
        }

        $(gridName).jqGrid('GridUnload');
        $(gridName).jqGrid('clearGridData');
        $(gridName).trigger("reloadGrid");
        $(gridName).jqGrid(
            {
                datatype: 'local',
                data: myData,
                async: false,
                jsonReader: {
                    root: 'rows',
                    page: 'page',
                    repeatitems: false
                },
                colNames: gridColNames,
                colModel: gridColModel,
                pager: jQuery(pagerName),
                rowNum: rowNum,
                hidegrid: false,
                height: height,
                width: width,
                recordtext: "{0} - {1} de {2} elementos",
                emptyrecords: 'No hay resultados',
                sortname: sortName,
                sortorder: sortOrder, //Default SortOrder.
                viewrecords: true,
                rowheight: "100",
                rownumbers: rownumbers,
                gridview: true,
                multiselect: multiselect,
                view: {
                    caption: "Ver Registro",
                    bClose: "Cerrar"
                },
                loadError: function (jqXHR, textStatus, errorThrown) {
                    alert('HTTP status code: ' + jqXHR.status + '\n' +
                        'textStatus: ' + textStatus + '\n' +
                        'errorThrown: ' + errorThrown);
                    alert('HTTP message body (jqXHR.responseText): ' + '\n' + jqXHR.responseText);
                }
            });
        $(gridName).jqGrid('navGrid', pagerName, { edit: false, add: false, del: false, search: false, refresh: false, viewrecords: true });
    }

    // Public section
    return { // NOTE: module export breaks chaining
        // set init as first property
        init: function () {
            if (!this.listenersInitialized) {
                // Attach spinner to AJAX requests                               
                createEmptyGrid();                                

                $("#btnUploadTarifas").on("click", function (e) {
                    e.preventDefault();
                    insertarConciliaciones();
                });
            }
        },

        name: "IMPORTARRECONCILIACIONPROVEEDORES",
        listenersInitialized: false,
        readFile: function (fileName) {            
            var gridName = "#gridProveedores";
            var pagerName = "#pagerProveedores";

            var url = "importar_reconciliacion_proveedores.aspx/readFile";
            var params = '{fileFullName:"' + fileName + '"}';
            var errMsg = "Error al leer archivo: ";

            var response = genericCallWebMethod(url, params, errMsg, false);
            console.log(response)
            if (response.responseArray.length > 1) {
                var sortName = "Id";
                var sortOrder = "asc";                

                createGrid(gridName, pagerName, response.responseArray, gridColumns, gridColumnsModel, sortName, sortOrder, "1500", "450");

                $(gridName)
                    .jqGrid("setGridParam",
                        {
                            ondblClickRow: function (rowid, iRow, iCol, e) {
                                var data = $(gridName).getRowData(rowid);                                
                                jQuery(gridName)
                                    .jqGrid("viewGridRow", rowid, { caption: "Ver Registro", bClose: "Cerrar" });
                                
                            }
                        });
            } else {
                alert("Ocurrio un error al leer archivo o el archivo esta vacio.");
            }
        }
    };
}(jQuery)); // Pass globals to inner object scope as parameters to avoid lookups and set specific usages

// Call object(s) init to be executed first
$(document).ready(function () {
    IMPORTARRECONCILIACIONPROVEEDORES.init();
});
