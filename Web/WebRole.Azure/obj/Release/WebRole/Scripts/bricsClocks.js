$(document).ready(function () {
    new UpdateClocks();
    setInterval("UpdateClocks()", 30000);
});

function UpdateClocks() {
    var hours = new Date().getUTCHours();
    var mins = new Date().getUTCMinutes();

    var johannesburg_offset_pos = 2;
    var shanghai_offset_pos = 8;
    var mumbai_offset_pos = 5
    var moscow_offset_pos = 3;
    var saupaulo_offset_neg = 4;

    //check for midnight condition - johannesburg
    if (hours + johannesburg_offset_pos > 24) {
        var johannesburg_hours = hours - 24;
    }
    else {
        var johannesburg_hours = hours;
    }

    //check for midnight condition - shanghai
    if (hours + shanghai_offset_pos > 24) {
        var shanghai_hours = hours - 24;
    }
    else {
        var shanghai_hours = hours;
    }

    //check for midnight condition - moscow
    if (hours + moscow_offset_pos > 24) {
        var moscow_hours = hours - 24;
    }
    else {
        var moscow_hours = hours;
    }

    //check for midnight condition - saupaulo
    if (saupaulo_offset_neg - hours < 0) {
        var saupaulo_hours = 0 + hours;
    }
    else {
        var saupaulo_hours = hours;
    }

    //format minutes - all except mumbai
    if (mins > 0 && mins < 10) {
        var min_string = "0" + mins;
    }
    else {
        var min_string = mins;
    }

    var johannesburg_time = johannesburg_hours + johannesburg_offset_pos + ":" + min_string;
    var shanghai_time = shanghai_hours + shanghai_offset_pos + ":" + min_string;
    var moscow_time = moscow_hours + moscow_offset_pos + ":" + min_string;
    var saupaulo_time = saupaulo_hours - saupaulo_offset_neg + ":" + min_string;

    var mumbai_min = mins + 30; //special case for india - add 30 minutes in addition to hour offset to utc time
    if (mumbai_min > 60) {
        mumbai_min = mumbai_min - 60;
        var mumbai_hour = hours + mumbai_offset_pos + 1;
    }
    else {
        mumbai_hour = hours + mumbai_offset_pos;
    }

    //format minutes - mumbai
    if (mumbai_min > 0 && mumbai_min < 10) {
        var mumbai_min_string = "0" + mumbai_min;
    }
    else {
        var mumbai_min_string = mumbai_min;
    }

    //check for midnight condition - mumbai
    if (mumbai_hour + mumbai_offset_pos > 24) {
        var mumbai_hours = mumbai_hour - 24;
    }
    else {
        var mumbai_hours = mumbai_hour;
    }

    var mumbai_time = mumbai_hour + ":" + mumbai_min_string;

    $("#zone-johannesburg").html(johannesburg_time);

    $("#zone-shanghai").html(shanghai_time);

    $("#zone-mumbai").html(mumbai_time);

    $("#zone-moscow").html(moscow_time);

    $("#zone-saupolo").html(saupaulo_time);
}