var weatherData;
var request = new XMLHttpRequest();
var date = new Date();

updateCount(8);

function updateCount(num) {
    "use strict";
    $('#numDays').text(Math.floor(num));
    loadData(Math.floor(num), true);
    $("#detailDisplay").hide();
}

//myID: 8d337bba226b07650ccf3b3a4fab298f
//oldID: 846d3b48355b6e95813bed8fb29f0fb3
function loadData(cnt, imperial) {
    "use strict";
    request.open('GET', 'http://api.openweathermap.org/data/2.5/forecast/daily?q=Salt+Lake+City,us&' + (imperial ? 'units=imperial' : '') + '&cnt=' + cnt + '&appid=8d337bba226b07650ccf3b3a4fab298f');

    request.onload = loadComplete;
    request.send();
}

function loadComplete(evt) {
    "use strict";
    weatherData = JSON.parse(request.responseText);

    var workingDate = new Date(date.getTime()),
        week = $('#weekDisplay');

    week.empty();
    for (var i = 0; i < workingDate.getDay(); i++) {
        week.append(`<li class="dayPadding"></li>`);
    }
    for (var i = 0; i < weatherData.cnt; i++) {
        var id = '' + i;
        var dayOverview = $("<li></li>", {
            'class': "dayOverview",
            id: id,
            date: dateToDay(workingDate.getDay()) + " " + (workingDate.getMonth() + 1) + "/" + workingDate.getDate() + "/" + workingDate.getFullYear(),
            on: {
                "click": function (event) {
                    updateDetails($(this).attr('id'), $(this).attr('date'));
                }
            }
        });
        var dayHeader = $("<div></div>", {
            class: "dayHeader"
        });
        var overviewDay = $("<div></div>", {
            class: "overviewDay",
            text: dateToDay(workingDate.getDay())
        });
        var overviewDate = $("<div></div>", {
            class: "overviewDate",
            text: (workingDate.getMonth() + 1) + "/" + workingDate.getDate() + "/" + workingDate.getFullYear()
        });

        var dayData = $("<div></div>", {
            class: "dayData"
        });
        var overviewConditions = $("<div></div>", {
            class: "overviewConditions",
            text: weatherData.list[i].weather[0].main
        });
        var overviewConditionsDesc = $("<div></div>", {
            class: "overviewConditionsDesc",
            text: weatherData.list[i].weather[0].description
        });
        var overviewCurrentTemp = $("<div></div>", {
            class: "overviewCurrentTemp",
            text: weatherData.list[i].temp.day
        });
        
        var overviewImg = $("<img>", {
            class: "overviewImage",
            src: "resources/clipart/" + weatherData.list[i].weather[0].main + ".png"
        });

        dayData.append(overviewConditions);
        dayData.append(overviewConditionsDesc);
        dayData.append(overviewCurrentTemp);

        dayHeader.append(overviewDay);
        dayHeader.append(overviewDate);

        dayOverview.append(dayHeader);
        dayOverview.append(dayData);
        dayOverview.append(overviewImg);

        week.append(dayOverview);

        workingDate.setDate(workingDate.getDate() + 1);
    }
}

function updateDetails(index, date) {
    var data = weatherData.list[parseInt(index)];

    var detailDiv = $('#detailDisplay');
    $('#detailDisplay').css('background-image', "url(resources/backgrounds/" + data.weather[0].main + ".jpg)");
                            
    $("#detailDisplay").show();
    
    $("#detailDisplay").on("click", function () {
        $("#detailDisplay").hide();
    });
    detailDiv.empty();

    var miscBlock = $("<div></div>", {
        id: 'detailMiscBlock',
    });
    miscBlock.hide();

    if (data.clouds) {
        var clouds = $("<div></div>", {
            id: 'detailCloudCover',
            text: "Cloud cover: " + data.clouds + "%"
        });
        miscBlock.append(clouds);
        miscBlock.show();
    }

    if (data.rain) {
        var rain = $("<div></div>", {
            id: 'detailRain',
            text: "Rain: " + data.rain + '"'
        });
        miscBlock.append(rain);
        miscBlock.show();
    }
    if (data.snow) {
        var snow = $("<div></div>", {
            id: 'detailSnow',
            text: "Snow: " + data.snow + '"'
        });
        miscBlock.append(snow);
        miscBlock.show();
    }

    if (data.humidity) {
        var humidity = $("<div></div>", {
            id: 'detailHumidity',
            text: "Humidity: " + data.humidity + "%"
        });
        miscBlock.append(humidity);
        miscBlock.show();
    }

    if (data.pressure) {
        var pressure = $("<div></div>", {
            id: 'detailPressure',
            text: "Pressure: " + data.pressure + " mmHg(Torr)"
        });
        miscBlock.append(pressure);
        miscBlock.show();
    }

    if (data.speed) {
        var speed = $("<div></div>", {
            id: 'detailWindSpeed',
            text: "WindSpeed: " + data.speed + " Mph"
        });
        miscBlock.append(speed);
        miscBlock.show();
    }
    detailDiv.append(miscBlock);

    if (data.temp) {
        var temp = $("<ul></ul>", {
            id: 'detailTempList',
            text: ""
        });

        if (data.temp.min) {
            var tmp = $("<li></li>", {
                class: 'detailTemps',
                id: 'detailTempMin',
                html: "Tempurature-min: <br/>" + data.temp.min + "&deg; F"
            });
            temp.append(tmp);
        }
        if (data.temp.max) {
            var tmp = $("<li></li>", {
                class: 'detailTemps',
                id: 'detailTempMax',
                html: "Tempurature-max: <br/>" + data.temp.max + "&deg; F"
            });
            temp.append(tmp);
        }
        if (data.temp.morn) {
            var tmp = $("<li></li>", {
                class: 'detailTemps',
                id: 'detailTempMorn',
                html: "Tempurature-morning: <br/>" + data.temp.morn + "&deg; F"
            });
            temp.append(tmp);
        }
        if (data.temp.day) {
            var tmp = $("<li></li>", {
                class: 'detailTemps',
                id: 'detailTempDay',
                html: "Tempurature-day: <br/>" + data.temp.day + "&deg; F"
            });
            temp.append(tmp);
        }
        if (data.temp.eve) {
            var tmp = $("<li></li>", {
                class: 'detailTemps',
                id: 'detailTempEve',
                html: "Tempurature-evening: <br/>" + data.temp.eve + "&deg; F"
            });
            temp.append(tmp);
        }
        if (data.temp.night) {
            var tmp = $("<li></li>", {
                class: 'detailTemps',
                id: 'detailTempNight',
                html: "Tempurature-night: <br/>" + data.temp.night + "&deg; F"
            });
            temp.append(tmp);
        }

        detailDiv.append(temp);
    }

    if (data.weather) {
        var weath = $("<ul></ul>", {
            id: 'detailWeatherList',
            text: ""
        });

        for (var i = 0; i < data.weather.length; i++) {
            var ther = $("<li></li>", {
                class: 'detailWeathers',
                id: 'detailWeather' + i,
                text: data.weather[i].description
            });
            weath.append(ther);
        }

        detailDiv.append(weath);
    }
    
    var dt = $("<div></div>", {
                id: 'detailDate',
                text: date
            });
    detailDiv.append(dt);

}

function dateToDay(dayNum) {
    "use strict";
    switch (dayNum) {
    case 0:
        return "Sunday";
    case 1:
        return "Monday";
    case 2:
        return "Tuesday";
    case 3:
        return "Wednesday";
    case 4:
        return "Thursday";
    case 5:
        return "Friday";
    case 6:
        return "Saturday";
    }
}