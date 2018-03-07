function onReady(eventObject) {
    $("li[id^=\"team-selector-\"]").each(function(index) {
        var $current = $(this);

        var sCurveRank = parseInt($current.attr("id").replace("team-selector-", ""));

        var team = allTeams[sCurveRank - 1];

        $current.data("team", allTeams[sCurveRank - 1]);

        if (team.pickedByPlayer == 0) {
            $current.css("cursor", "pointer");
            $current.click(toggleSelectTeam);
        }
    });

    $(window).scroll(handleOnScroll);

    $("#buttonSelectBeforeConfirm").button();

    $("#fourPanelRadioSection").buttonset();

    $('#bracketOneRadio').click(function() {
        showBracket(1);
    });
    
    $('#bracketTwoRadio').click(function() {
        showBracket(2);
    });
    
    $('#bracketThreeRadio').click(function() {
        showBracket(3);
    });
    
    $('#bracketFourRadio').click(function() {
        showBracket(4);
    });

    $("#twoPanelRadioSection").buttonset();

    $('#bracketOneAndFourRadio').click(function() {
        showTwoBracketPanel(1);
    });

    $('#bracketTwoAndThreeRadio').click(function() {
        showTwoBracketPanel(2);
    });

    updatePointsAvailableSpan();

    if (!commonMethods.isMobileDevice()) {
        untabify();
    }
    else {
        $(document.body).bind("orientationchange", updateOrientation);
        updateOrientation();
    }
}

function updateOrientation()
{
    if (window.orientation == 90 || window.orientation == -90) {
        twoTabs();
    }
    else {
        fourTabs();
    }
}

function handleOnScroll() {
    adjustFixedTop(true);
}

function adjustFixedTop(animate) {
    if (commonMethods.isMobileDevice()) {
        if (animate) {
            $("#fixedTop").css("position", "absolute").css("border-bottom", "solid 1px #aaa").animate({ top: window.pageYOffset + "px" }, 250);
        }
        else {
            $("#fixedTop").css("position", "absolute").css("border-bottom", "solid 1px #aaa").css("top", window.pageYOffset + "px");
        }
    }
    else {
        $("#fixedTop").css("position", "static").css("border-bottom", "none");
    }

}

function adjustTopSpacer() {
    if (commonMethods.isMobileDevice()) {
        var newHeight = $("#fixedTopStaticSection").height();
        
        $("#fixedTopPlaceholder").css("height", newHeight + "px").show();
    }
    else {
        $("#fixedTopPlaceholder").hide();
    }
}

function fourTabs() {
    $("#bracket1").css("float", "").css("width", "").hide();
    $("#bracket4").css("float", "").css("width", "").hide();
    $("#bracket2").css("float", "").css("width", "").hide();
    $("#bracket3").css("float", "").css("width", "").hide();

    $("#bracket1Header").hide();
    $("#bracket2Header").hide();
    $("#bracket3Header").hide();
    $("#bracket4Header").hide();

    $("#brackets1and4").show();
    $("#brackets2and3").show();

    $("#twoPanelRadioSection").hide();
    $("#fourPanelRadioSection").show();

    $("#topLevelHeader").hide();
    $(document.body).css("margin", "0px");
    $(document.body).css("padding", "0px");

    $("#bracketOneRadio").click();

    $("#fourPanelRadioSection").buttonset("refresh");

    adjustFixedTop(false);
    adjustTopSpacer();
}

function twoTabs() {
    $("#bracket1").css("float", "left").css("width", "50%").show();
    $("#bracket4").css("float", "left").css("width", "50%").show();
    $("#bracket2").css("float", "left").css("width", "50%").show();
    $("#bracket3").css("float", "left").css("width", "50%").show();

    $("#bracket1Header").show();
    $("#bracket2Header").show();
    $("#bracket3Header").show();
    $("#bracket4Header").show();

    $("#brackets1and4").hide();
    $("#brackets2and3").hide();

    $("#fourPanelRadioSection").hide();
    $("#twoPanelRadioSection").show();
    
    $("#topLevelHeader").hide();
    $(document.body).css("margin", "0px");
    $(document.body).css("padding", "0px");

    $("#bracketOneAndFourRadio").click();

    $("#twoPanelRadioSection").buttonset("refresh");

    adjustFixedTop(false);
    adjustTopSpacer();
}

function untabify() {
    $("#bracket1").css("float", "left").css("width", "25%").show();
    $("#bracket4").css("float", "left").css("width", "25%").show();
    $("#bracket2").css("float", "left").css("width", "25%").show();
    $("#bracket3").css("float", "left").css("width", "25%").show();

    $("#bracket1Header").show();
    $("#bracket2Header").show();
    $("#bracket3Header").show();
    $("#bracket4Header").show();

    $("#brackets1and4").show();
    $("#brackets2and3").show();

    $("#twoScreenSplit").css("display", "none");

    $("#twoPanelRadioSection").hide();
    $("#fourPanelRadioSection").hide();

    $("#topLevelHeader").show();
    $(document.body).css("margin", "");
    $(document.body).css("padding", "");

    adjustFixedTop(false);
    adjustTopSpacer();
}

function showBracket(bracketNumber) {
    if (bracketNumber == 1) {
        $("#bracket1").show();
    }
    else {
        $("#bracket1").hide();
    }

    if (bracketNumber == 4) {
        $("#bracket4").show();
    }
    else {
        $("#bracket4").hide();
    }

    if (bracketNumber == 2) {
        $("#bracket2").show();
    }
    else {
        $("#bracket2").hide();
    }

    if (bracketNumber == 3) {
        $("#bracket3").show();
    }
    else {
        $("#bracket3").hide();
    }

    setTimeout(function() { adjustFixedTop(false); }, 10);
}

function showTwoBracketPanel(twoBracketPanelNumber) {
    if (twoBracketPanelNumber == 1) {
        $("#brackets1and4").show();
    }
    else {
        $("#brackets1and4").hide();
    }

    if (twoBracketPanelNumber == 2) {
        $("#brackets2and3").show();
    }
    else {
        $("#brackets2and3").hide();
    }

    setTimeout(function() { adjustFixedTop(false); }, 10);
}

function toggleSelectTeam(eventObject) {
    var $targetSelector = $(eventObject.target);

    if (eventObject.target.nodeName == "SPAN") {
        $targetSelector = $targetSelector.parent();
    }

    var team = $targetSelector.data("team");

    if (team.pickedByPlayer == 0) {
        if ($targetSelector.hasClass("selected")) {
            $targetSelector.removeClass("selected");

            currentStatus.pointsAvailable = currentStatus.pointsAvailable + team.cost;
            currentStatus.removeTeam(team.sCurveRank);
        }
        else {
            if ($targetSelector.data("team").cost <= currentStatus.pointsAvailable) {
                $targetSelector.addClass("selected");

                currentStatus.pointsAvailable = currentStatus.pointsAvailable - team.cost;
                currentStatus.addTeam(team.sCurveRank);
            }
            else {
                showError("Cannot afford " + team.teamName);
            }
        }
    }

    updateSelectedTeamsField();
    updatePointsAvailableSpan();

    return false;
}

function updatePointsAvailableSpan()
{
    $("#spanPointsRemaining").text(currentStatus.pointsAvailable);

    $("li[id^=\"team-selector-\"] > span[class=\"cost\"]").each(function() {
        var team = $(this).parent("li[id^=\"team-selector-\"]").data("team");

        if (team.pickedByPlayer == 0 && team.cost > currentStatus.pointsAvailable && !currentStatus.isTeamSelected(team.sCurveRank)) {
            // Can't afford team, make cost gray
            $(this).css("background-color", "#666666");
        }
        else {
            $(this).css("background-color", "");
        }
    });

    if (currentStatus.pointsAvailable < 0 || currentStatus.pointsAvailable > currentStatus.currentRound.maxHoldOverPoints) {
        // Disable the submit button
        $("#buttonSelectBeforeConfirm").button("disable");
        $("#buttonSubmit").attr('disabled', 'disabled');
        $("#spanPointsRemaining").css("background-color", "#CC0000");
    }
    else {
        // Enable the submit button
        $("#buttonSelectBeforeConfirm").button("enable");
        $("#buttonSubmit").removeAttr('disabled');
        $("#spanPointsRemaining").css("background-color", "#00CC00");
    }
}

function updateSelectedTeamsField() {
    $("#hiddenSelectedTeams").val(currentStatus.selectedTeams.join(","));
}

function confirmSelect() {
    $("#dialog-confirm").dialog({
        resizable: false,
        height: 160,
        modal: true,
        buttons: {
            Yes: function() {
                $(this).dialog("close");
                $("#buttonSubmit").click();
            },
            No: function() {
                $(this).dialog("close");
            }
        }
    });

    return false;
}

var errorHidingTimeoutId;

function showError(text) {
    if (errorHidingTimeoutId != null) {
        clearTimeout(errorHidingTimeoutId);
    }

    if (commonMethods.isMobileDevice()) {
        $("#divError").text(text).slideDown("fast", function() { errorHidingTimeoutId = setTimeout(hideError, 2000); });
    }
    else {
        $("#divError").text(text).fadeIn("slow", function() { errorHidingTimeoutId = setTimeout(hideError, 2000); });
    }
}

function hideError() {
    if (commonMethods.isMobileDevice()) {
        $("#divError").slideUp("fast");
    }
    else {
        $("#divError").fadeOut("slow");
    }
}

var currentStatus =
{
    "pointsAvailable": 0,
    "currentRound": null,
    "currentPlayer": null,
    "selectedTeams": [],
    "addTeam": function(sCurveRank) {
        for (var i = 0; i < this.selectedTeams.length; i++) {
            if (this.selectedTeams[i] == sCurveRank) {
                return;
            }
        }

        this.selectedTeams.push(sCurveRank);
    },
    "removeTeam": function(sCurveRank) {
        var existingIndex = -1;

        for (var i = 0; i < this.selectedTeams.length; i++) {
            if (this.selectedTeams[i] == sCurveRank) {
                existingIndex = i;
                break;
            }
        }

        if (existingIndex >= 0) {
            this.selectedTeams.splice(existingIndex, 1);
        }
    },
    "isTeamSelected" : function(sCurveRank)
    {
        for (var i = 0; i < this.selectedTeams.length; i++) {
            if (this.selectedTeams[i] == sCurveRank) {
                return true;
            }
        }
        
        return false;
    }
};