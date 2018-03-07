function onReady(eventObject) {
    var $rounds = $("div.round");

    if ($rounds.length > 0) {
        if (commonMethods.isMobileDevice()) {
            // Only show the current round
            $rounds.hide();
            $("div.currentRound").show();
        }
        else {
            var roundWidth = 80 / $rounds.length;

            $rounds.css("width", roundWidth + "%");
        }

        setTimeout("window.location.reload();", 30000);
    }

    $("a").button();
}
