﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProjectsList.ascx.cs" Inherits="FrontEndSite.UserControls.ucProjectsList" %>

<link href="../Content/ListViewStyle.css" rel="stylesheet" />

<div class="col-md-12">
    <label id="lbError" runat="server" class="errMsg" visible="false"></label>
</div>

<div id="Home-Projects" class="list-projects col-md-12">
    <asp:HiddenField ID="hfMenuId" runat="server" ClientIDMode="Static"/>
    <asp:HiddenField ID="hfScanTime" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfFirstScanTime" runat="server" ClientIDMode="Static" />

    <asp:ListView ID="lvProjects" runat="server" OnItemDataBound="lvProjects_ItemDataBound">
        <EmptyDataTemplate>
            <div class="NoDataStyle">No data was returned.</div>
        </EmptyDataTemplate>
        <EmptyItemTemplate>
            <div class="NoDataStyle">No data was returned.</div>
        </EmptyItemTemplate>

        <ItemTemplate>
            <div class="col-md-4 ProjectItem">
                <asp:Image ID="imageLink" runat="server" ImageUrl='<%# Eval("ImageLink") %>' />
                <div class="ProjectItem-Name">
                    <asp:HyperLink ID="hplName" runat="server" Text='<%# Eval("Name_VN") %>' NavigateUrl='<%# Eval("Id") %>' />
                    <asp:HyperLink ID="hplCategory" runat="server" Text='<%# Eval("MenuName_VN") %>' NavigateUrl='<%# string.Format("~/{0}?MenuId={1}", "Projects.aspx", Eval("MenuId")) %>'/>
                </div>
            </div>
        </ItemTemplate>
    </asp:ListView>
</div>

<div id="Home-Project-Bottom" class="col-md-12">
    <img src="../Images/loading.gif" />
</div>

<script type="text/javascript">
    var isWait = false;

    $(function () {
        var firstScanTime = $("#hfFirstScanTime").val();
        $("#hfScanTime").val(firstScanTime);
    });

    //$("#Home-Projects").on("click", ".ProjectItem a, .ProjectItem img", function (e) {
    //    e.preventDefault();
    //    if (isWait)
    //    {
    //        return false;
    //    }

        
    //});

    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() >= $(document).height()) {

            if (isWait == true) {
                return;
            }

            isWait = false;

            $("#Home-Project-Bottom").fadeIn("fast");
            //$("#Home-Project-Bottom").show();
            //-- Start load page index
            var $scanTime = $("#hfScanTime").val();
            StartGetProjectItems($scanTime);
            //---

        }
    });

    function StartGetProjectItems(scanTime) {
        if (isWait == true) {
            return;
        }

        var menuId = $("#hfMenuId").val();

        if (scanTime.length == 1 && scanTime != "0") {
            isWait = false;
            $("#Home-Project-Bottom").fadeOut("slow");

            return;
        }

        var data = {
            'MenuId': menuId,
            'ScanTime': scanTime,
            "ScanType": "MainProjects"
        };

        $.ajax({
            url: "/WebHandler/GetProjectsHandler.ashx",
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            async: false,
            data: data,
            success: function (result) {
                if (result == undefined) {
                    isWait = false;
                    $("#Home-Project-Bottom").fadeOut("slow");

                    return;
                }

                if (result.Code < 0) {
                    isWait = false;
                    $("#Home-Project-Bottom").fadeOut("slow");
                    alert(result.Message);

                    return;
                };

                var $dataSource = result.Data;
                StartBindingProjects($dataSource);
                //---
                $("#hfScanTime").val(result.ScanTime);
            },
            error: function () {
                isWait = false;
            }
        });
    }

    function StartBindingProjects(dataSource) {
        if (dataSource == undefined) {
            isWait = false;
            $("#Home-Project-Bottom").fadeOut("slow");

            return;
        }

        $.each(dataSource, function (index, item) {
            if (item != undefined) {
                var lvItem = '<div class="col-md-4 ProjectItem">';
                var itemImage = item.ImageLink;
                var itemName = '<div class="ProjectItem-Name">' + item.Name + item.MenuName + '</div>';

                lvItem += itemImage;
                lvItem += itemName;
                lvItem += '</div>';

                $("#Home-Projects").append(lvItem);
            }
        });

        isWait = false;
        $("#Home-Project-Bottom").fadeOut("slow");
    }

</script>