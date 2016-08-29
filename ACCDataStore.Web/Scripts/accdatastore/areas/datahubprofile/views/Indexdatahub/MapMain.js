var map;
var data1;
var datag;
var glasgow = {
    dataZones: ['S01003025', 'S01003026', 'S01003027', 'S01003028', 'S01003029', 'S01003030', 'S01003031', 'S01003032', 'S01003033', 'S01003034', 'S01003035', 'S01003036', 'S01003037', 'S01003038', 'S01003039', 'S01003040', 'S01003041', 'S01003042', 'S01003043', 'S01003044', 'S01003045', 'S01003046', 'S01003047', 'S01003048', 'S01003049', 'S01003050', 'S01003051', 'S01003052', 'S01003053', 'S01003054', 'S01003055', 'S01003056', 'S01003057', 'S01003058', 'S01003059', 'S01003060', 'S01003061', 'S01003062', 'S01003063', 'S01003064', 'S01003065', 'S01003066', 'S01003067', 'S01003068', 'S01003069', 'S01003070', 'S01003071', 'S01003072', 'S01003073', 'S01003074', 'S01003075', 'S01003076', 'S01003077', 'S01003078', 'S01003079', 'S01003080', 'S01003081', 'S01003082', 'S01003083', 'S01003084', 'S01003085', 'S01003086', 'S01003087', 'S01003088', 'S01003089', 'S01003090', 'S01003091', 'S01003092', 'S01003093', 'S01003094', 'S01003095', 'S01003096', 'S01003097', 'S01003098', 'S01003099', 'S01003100', 'S01003101', 'S01003102', 'S01003103', 'S01003104', 'S01003105', 'S01003106', 'S01003107', 'S01003108', 'S01003109', 'S01003110', 'S01003111', 'S01003112', 'S01003113', 'S01003114', 'S01003115', 'S01003116', 'S01003117', 'S01003118', 'S01003119', 'S01003120', 'S01003121', 'S01003122', 'S01003123', 'S01003124', 'S01003125', 'S01003126', 'S01003127', 'S01003128', 'S01003129', 'S01003130', 'S01003131', 'S01003132', 'S01003133', 'S01003134', 'S01003135', 'S01003136', 'S01003137', 'S01003138', 'S01003139', 'S01003140', 'S01003141', 'S01003142', 'S01003143', 'S01003144', 'S01003145', 'S01003146', 'S01003147', 'S01003148', 'S01003149', 'S01003150', 'S01003151', 'S01003152', 'S01003153', 'S01003154', 'S01003155', 'S01003156', 'S01003157', 'S01003158', 'S01003159', 'S01003160', 'S01003161', 'S01003162', 'S01003163', 'S01003164', 'S01003165', 'S01003166', 'S01003167', 'S01003168', 'S01003169', 'S01003170', 'S01003171', 'S01003172', 'S01003173', 'S01003174', 'S01003175', 'S01003176', 'S01003177', 'S01003178', 'S01003179', 'S01003180', 'S01003181', 'S01003182', 'S01003183', 'S01003184', 'S01003185', 'S01003186', 'S01003187', 'S01003188', 'S01003189', 'S01003190', 'S01003191', 'S01003192', 'S01003193', 'S01003194', 'S01003195', 'S01003196', 'S01003197', 'S01003198', 'S01003199', 'S01003200', 'S01003201', 'S01003202', 'S01003203', 'S01003204', 'S01003205', 'S01003206', 'S01003207', 'S01003208', 'S01003209', 'S01003210', 'S01003211', 'S01003212', 'S01003213', 'S01003214', 'S01003215', 'S01003216', 'S01003217', 'S01003218', 'S01003219', 'S01003220', 'S01003221', 'S01003222', 'S01003223', 'S01003224', 'S01003225', 'S01003226', 'S01003227', 'S01003228', 'S01003229', 'S01003230', 'S01003231', 'S01003232', 'S01003233', 'S01003234', 'S01003235', 'S01003236', 'S01003237', 'S01003238', 'S01003239', 'S01003240', 'S01003241', 'S01003242', 'S01003243', 'S01003244', 'S01003245', 'S01003246', 'S01003247', 'S01003248', 'S01003249', 'S01003250', 'S01003251', 'S01003252', 'S01003253', 'S01003254', 'S01003255', 'S01003256', 'S01003257', 'S01003258', 'S01003259', 'S01003260', 'S01003261', 'S01003262', 'S01003263', 'S01003264', 'S01003265', 'S01003266', 'S01003267', 'S01003268', 'S01003269', 'S01003270', 'S01003271', 'S01003272', 'S01003273', 'S01003274', 'S01003275', 'S01003276', 'S01003277', 'S01003278', 'S01003279', 'S01003280', 'S01003281', 'S01003282', 'S01003283', 'S01003284', 'S01003285', 'S01003286', 'S01003287', 'S01003288', 'S01003289', 'S01003290', 'S01003291', 'S01003292', 'S01003293', 'S01003294', 'S01003295', 'S01003296', 'S01003297', 'S01003298', 'S01003299', 'S01003300', 'S01003301', 'S01003302', 'S01003303', 'S01003304', 'S01003305', 'S01003306', 'S01003307', 'S01003308', 'S01003309', 'S01003310', 'S01003311', 'S01003312', 'S01003313', 'S01003314', 'S01003315', 'S01003316', 'S01003317', 'S01003318', 'S01003319', 'S01003320', 'S01003321', 'S01003322', 'S01003323', 'S01003324', 'S01003325', 'S01003326', 'S01003327', 'S01003328', 'S01003329', 'S01003330', 'S01003331', 'S01003332', 'S01003333', 'S01003334', 'S01003335', 'S01003336', 'S01003337', 'S01003338', 'S01003339', 'S01003340', 'S01003341', 'S01003342', 'S01003343', 'S01003344', 'S01003345', 'S01003346', 'S01003347', 'S01003348', 'S01003349', 'S01003350', 'S01003351', 'S01003352', 'S01003353', 'S01003354', 'S01003355', 'S01003356', 'S01003357', 'S01003358', 'S01003359', 'S01003360', 'S01003361', 'S01003362', 'S01003363', 'S01003364', 'S01003365', 'S01003366', 'S01003367', 'S01003368', 'S01003369', 'S01003370', 'S01003371', 'S01003372', 'S01003373', 'S01003374', 'S01003375', 'S01003376', 'S01003377', 'S01003378', 'S01003379', 'S01003380', 'S01003381', 'S01003382', 'S01003383', 'S01003384', 'S01003385', 'S01003386', 'S01003387', 'S01003388', 'S01003389', 'S01003390', 'S01003391', 'S01003392', 'S01003393', 'S01003394', 'S01003395', 'S01003396', 'S01003397', 'S01003398', 'S01003399', 'S01003400', 'S01003401', 'S01003402', 'S01003403', 'S01003404', 'S01003405', 'S01003406', 'S01003407', 'S01003408', 'S01003409', 'S01003410', 'S01003411', 'S01003412', 'S01003413', 'S01003414', 'S01003415', 'S01003416', 'S01003417', 'S01003418', 'S01003419', 'S01003420', 'S01003421', 'S01003422', 'S01003423', 'S01003424', 'S01003425', 'S01003426', 'S01003427', 'S01003428', 'S01003429', 'S01003430', 'S01003431', 'S01003432', 'S01003433', 'S01003434', 'S01003435', 'S01003436', 'S01003437', 'S01003438', 'S01003439', 'S01003440', 'S01003441', 'S01003442', 'S01003443', 'S01003444', 'S01003445', 'S01003446', 'S01003447', 'S01003448', 'S01003449', 'S01003450', 'S01003451', 'S01003452', 'S01003453', 'S01003454', 'S01003455', 'S01003456', 'S01003457', 'S01003458', 'S01003459', 'S01003460', 'S01003461', 'S01003462', 'S01003463', 'S01003464', 'S01003465', 'S01003466', 'S01003467', 'S01003468', 'S01003469', 'S01003470', 'S01003471', 'S01003472', 'S01003473', 'S01003474', 'S01003475', 'S01003476', 'S01003477', 'S01003478', 'S01003479', 'S01003480', 'S01003481', 'S01003482', 'S01003483', 'S01003484', 'S01003485', 'S01003486', 'S01003487', 'S01003488', 'S01003489', 'S01003490', 'S01003491', 'S01003492', 'S01003493', 'S01003494', 'S01003495', 'S01003496', 'S01003497', 'S01003498', 'S01003499', 'S01003500', 'S01003501', 'S01003502', 'S01003503', 'S01003504', 'S01003505', 'S01003506', 'S01003507', 'S01003508', 'S01003509', 'S01003510', 'S01003511', 'S01003512', 'S01003513', 'S01003514', 'S01003515', 'S01003516', 'S01003517', 'S01003518', 'S01003519', 'S01003520', 'S01003521', 'S01003522', 'S01003523', 'S01003524', 'S01003525', 'S01003526', 'S01003527', 'S01003528', 'S01003529', 'S01003530', 'S01003531', 'S01003532', 'S01003533', 'S01003534', 'S01003535', 'S01003536', 'S01003537', 'S01003538', 'S01003539', 'S01003540', 'S01003541', 'S01003542', 'S01003543', 'S01003544', 'S01003545', 'S01003546', 'S01003547', 'S01003548', 'S01003549', 'S01003550', 'S01003551', 'S01003552', 'S01003553', 'S01003554', 'S01003555', 'S01003556', 'S01003557', 'S01003558', 'S01003559', 'S01003560', 'S01003561', 'S01003562', 'S01003563', 'S01003564', 'S01003565', 'S01003566', 'S01003567', 'S01003568', 'S01003569', 'S01003570', 'S01003571', 'S01003572', 'S01003573', 'S01003574', 'S01003575', 'S01003576', 'S01003577', 'S01003578', 'S01003579', 'S01003580', 'S01003581', 'S01003582', 'S01003583', 'S01003584', 'S01003585', 'S01003586', 'S01003587', 'S01003588', 'S01003589', 'S01003590', 'S01003591', 'S01003592', 'S01003593', 'S01003594', 'S01003595', 'S01003596', 'S01003597', 'S01003598', 'S01003599', 'S01003600', 'S01003601', 'S01003602', 'S01003603', 'S01003604', 'S01003605', 'S01003606', 'S01003607', 'S01003608', 'S01003609', 'S01003610', 'S01003611', 'S01003612', 'S01003613', 'S01003614', 'S01003615', 'S01003616', 'S01003617', 'S01003618', 'S01003619', 'S01003620', 'S01003621', 'S01003622', 'S01003623', 'S01003624', 'S01003625', 'S01003626', 'S01003627', 'S01003628', 'S01003629', 'S01003630', 'S01003631', 'S01003632', 'S01003633', 'S01003634', 'S01003635', 'S01003636', 'S01003637', 'S01003638', 'S01003639', 'S01003640', 'S01003641', 'S01003642', 'S01003643', 'S01003644', 'S01003645', 'S01003646', 'S01003647', 'S01003648', 'S01003649', 'S01003650', 'S01003651', 'S01003652', 'S01003653', 'S01003654', 'S01003655', 'S01003656', 'S01003657', 'S01003658', 'S01003659', 'S01003660', 'S01003661', 'S01003662', 'S01003663', 'S01003664', 'S01003665', 'S01003666', 'S01003667', 'S01003668', 'S01003669', 'S01003670', 'S01003671', 'S01003672', 'S01003673', 'S01003674', 'S01003675', 'S01003676', 'S01003677', 'S01003678', 'S01003679', 'S01003680', 'S01003681', 'S01003682', 'S01003683', 'S01003684', 'S01003685', 'S01003686', 'S01003687', 'S01003688', 'S01003689', 'S01003690', 'S01003691', 'S01003692', 'S01003693', 'S01003694', 'S01003695', 'S01003696', 'S01003697', 'S01003698', 'S01003699', 'S01003700', 'S01003701', 'S01003702', 'S01003703', 'S01003704', 'S01003705', 'S01003706', 'S01003707', 'S01003708', 'S01003709', 'S01003710', 'S01003711', 'S01003712', 'S01003713', 'S01003714', 'S01003715', 'S01003716', 'S01003717', 'S01003718'],
    intZones: ['S02000584', 'S02000585', 'S02000586', 'S02000587', 'S02000588', 'S02000589', 'S02000590', 'S02000591', 'S02000592', 'S02000593', 'S02000594', 'S02000595', 'S02000596', 'S02000597', 'S02000598', 'S02000599', 'S02000600', 'S02000601', 'S02000602', 'S02000603', 'S02000604', 'S02000605', 'S02000606', 'S02000607', 'S02000608', 'S02000609', 'S02000610', 'S02000611', 'S02000612', 'S02000613', 'S02000614', 'S02000615', 'S02000616', 'S02000617', 'S02000618', 'S02000619', 'S02000620', 'S02000621', 'S02000622', 'S02000623', 'S02000624', 'S02000625', 'S02000626', 'S02000627', 'S02000628', 'S02000629', 'S02000630', 'S02000631', 'S02000632', 'S02000633', 'S02000634', 'S02000635', 'S02000636', 'S02000637', 'S02000638', 'S02000639', 'S02000640', 'S02000641', 'S02000642', 'S02000643', 'S02000644', 'S02000645', 'S02000646', 'S02000647', 'S02000648', 'S02000649', 'S02000650', 'S02000651', 'S02000652', 'S02000653', 'S02000654', 'S02000655', 'S02000656', 'S02000657', 'S02000658', 'S02000659', 'S02000660', 'S02000661', 'S02000662', 'S02000663', 'S02000664', 'S02000665', 'S02000666', 'S02000667', 'S02000668', 'S02000669', 'S02000670', 'S02000671', 'S02000672', 'S02000673', 'S02000674', 'S02000675', 'S02000676', 'S02000677', 'S02000678', 'S02000679', 'S02000680', 'S02000681', 'S02000682', 'S02000683', 'S02000684', 'S02000685', 'S02000686', 'S02000687', 'S02000688', 'S02000689', 'S02000690', 'S02000691', 'S02000692', 'S02000693', 'S02000694', 'S02000695', 'S02000696', 'S02000697', 'S02000698', 'S02000699', 'S02000700', 'S02000701', 'S02000702', 'S02000703', 'S02000704', 'S02000705', 'S02000706', 'S02000707', 'S02000708', 'S02000709', 'S02000710', 'S02000711', 'S02000712', 'S02000713', 'S02000714', 'S02000715', 'S02000716']
}

$(document).ready(function () {
    //$.getJSON("http://localhost:2777/DatahubProfile/IndexDatahub/ApiJsonTest").done(function (data) {
    //    console.log("second success");
    //    data1 = data;
    //    loadData("Participating");
    //}).error(function (jqXHR, textStatus, errorThrown) {
    //    console.log("error " + textStatus);
    //    console.log("incoming Text " + jqXHR.responseText);
    //}).always(function () {
    //    console.log("complete");
    //});
    loadData("Participating");

    var dataZoneList = [];
    //$.getJSON(sContextPath + "DatahubProfile/IndexDatahub/GetDatazonesByCouncilName?councilName=Glasgow City").done(function (data) {
    //    data.forEach(function (item) {
    //        $.getJSON(sContextPath + "DatahubProfile/IndexDatahub/GetGeoJSON?id=" + item).done(function (data) {
    //            dataZoneList.push(data);
    //        })
    //    })
    //}).error(function (jqXHR, textStatus, errorThrown) {
    //    console.log("error " + textStatus);
    //    console.log("incoming Text " + jqXHR.responseText);
    //});
    
    $(document.body).on('click', '.a-close-popup-information', function () {
        $("#popup-information").hide(250);
    });
    InitSpinner();
    $("#selecteddataset").change(function () {
        var datasetname = $('#selecteddataset :selected').text();
        loadData(datasetname);
    });

    $("#thresholdViewBtn").click(function () {
        var low = parseInt($('#thresholdViewLow').val())
        var high = parseInt($('#thresholdViewHigh').val())
        threshholdView(datag, low, high);
    });

    $("#thresholdViewAvg").click(function () {
        $('#thresholdViewCenter').val(getAverage(datag.data));
    });
});

// initialize spinner on ajax loading
function InitSpinner() {
    $(document).ajaxSend(function () {
        $('#divSpinner').show();
    }).ajaxComplete(function () {
        $('#divSpinner').hide();
    }).ajaxError(function (e, xhr) {
        // do something on ajax error
    });
}

function onSlide(ui) {
    var totalWidth = $(".slider").width();
    $(".left-color").width((ui.values[0]) / 100 * totalWidth);
}

//Threshold view slider
function createSlider(centerValue) {
    $(".slider").slider({
        min: 0,
        max: 100,
        //step: 1,

        range: true,
        values: [centerValue, centerValue],
        slide: function (event, ui) {
            
            if (ui.values[1] < centerValue || ui.values[0] > centerValue) { return false };
            onSlide(ui)
            $("input.sliderValue[data-index=" + 0 + "]").val(centerValue - ui.values[0]);
            $("input.sliderValue[data-index=" + 1 + "]").val(-(centerValue - ui.values[1]));
        },
        create: function (event, ui) {
            onSlide({
                values: [centerValue, centerValue]
            });
        }
    })

    $("input.sliderValue").change(function () {
        var $this = $(this);
        if ($this.attr('data-index') == 0) {
            if ($this.val() < (0)) { $this.val(0) };
            if ($this.val() > centerValue) { $this.val(centerValue) };
            $(".slider").slider("values", $this.data("index"), (centerValue - parseInt($this.val())));
        }
        else {
            if ($this.val() < (0)) { $this.val(0) };
            if ($this.val() > (100 - centerValue)) { $this.val(100 - centerValue) };
            $(".slider").slider("values", $this.data("index"), (parseInt($this.val()) + centerValue));
        };
    });

    var averge = $('<label>|</label>').css('left', Math.round(getAverage(datag.data)) + '%');
    $('.slider').append(averge);
};

function threshholdView1(data, center, percentage) {
    map.data.setStyle(function (feature) {
        var low = [5, 69, 54];  // color of smallest datum
        var high = [151, 83, 34];   // color of largest datum

        var statisticdata = -1;

        var temp = feature.getProperty('DZ_CODE');

        for (var i = 0; i < data.datacode.length; i++) {
            if (data.datacode[i] == temp) {
                statisticdata = data.data[i];
                break;
            }

        }

        //var center = getAverage(datag.data);
        var color = 'yellow';
        var center = getAverage(datag.data);
        if (statisticdata < (center - percentage)) { color = 'red'; }
        if (statisticdata > (center + percentage)) { color = 'green'; }

        // determine whether to show this shape or not
        var showRow = true;
        if (statisticdata == -1) {
            showRow = false;
        }

        var outlineWeight = 0.5, zIndex = 1;
        if (feature.getProperty('datazone') === 'hover') {
            outlineWeight = zIndex = 2;
        }

        return {
            strokeWeight: outlineWeight,
            strokeColor: '#fff',
            zIndex: zIndex,
            fillColor: color,
            fillOpacity: 0.75,
            visible: showRow
        };
    });
};

function threshholdView(data, low, high) {
    map.data.setStyle(function (feature) {
        var statisticdata = -1;

        var temp = feature.getProperty('DZ_CODE');

        for (var i = 0; i < data.datacode.length; i++) {
            if (data.datacode[i] == temp) {
                statisticdata = data.data[i];
                break;
            }

        }

        //var center = getAverage(datag.data);
        var color = 'yellow';
        var center = getAverage(datag.data);
        if (statisticdata < (center - low)) { color = 'red'; }
        if (statisticdata > (center + high)) { color = 'green'; }

        // determine whether to show this shape or not
        var showRow = true;
        if (statisticdata == -1) {
            showRow = false;
        }

        var outlineWeight = 0.5, zIndex = 1;
        if (feature.getProperty('datazone') === 'hover') {
            outlineWeight = zIndex = 2;
        }

        return {
            strokeWeight: outlineWeight,
            strokeColor: '#fff',
            zIndex: zIndex,
            fillColor: color,
            fillOpacity: 0.75,
            visible: showRow
        };
    });
}

function getAverage(data) {
    var sum = 0;
    for (var i = 0; i < data.length; i++) {
        sum += parseInt(data[i], 10); //don't forget to add the base
    }
    var avg = sum / data.length;
    return avg;
}
// initialize map object
function InitMap(data) {
    datag = data;
    createSlider(Math.round(getAverage(datag.data)));
    $('#averageText').append(' ' + getAverage(datag.data))
    var mapCenter = new google.maps.LatLng(57.151810, -2.094451);
    var mapOptions = {
        zoom: 11,
        center: mapCenter
    }


    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    // Copypaste code
    var url = "https:\/\/maps.googleapis.com/maps/api/geocode/json?&address=Glasgow%2C%20uk";
    $.getJSON(url, function (result) {
        var loc = result.results[0].geometry.location;
        var sw = result.results[0].geometry.bounds.southwest;
        var ne = result.results[0].geometry.bounds.northeast;
        var bounds = new google.maps.LatLngBounds(sw, ne);

        map.setCenter(loc);
        map.fitBounds(bounds);
    })

    //map.data.loadGeoJson('https://dl.dropboxusercontent.com/u/870146/KML/V2/Datazone_with_Desc.json' + "?rand=" + (new Date()).valueOf());

    //map.data.addGeoJson(datazonejsondata);

    //glasgow.dataZones.forEach(function (reference) {
    //    var ulr2 = 'http:\/\/statistics.gov.scot/boundaries/' + reference + '.json';
    //    //console.log(ulr2);
    //    $.getJSON(ulr2, function (data) {
    //        map.data.addGeoJson(data);
    //    })
    //})

    //$.each(data1.result, function (i, item) { map.data.addGeoJson(item) })

    $.getJSON(sContextPath + "DatahubProfile/IndexDatahub/GetDatazonesByCouncilName?councilName=Glasgow City").done(function (data) {
        data.forEach(function (item) {
            $.getJSON(sContextPath + "DatahubProfile/IndexDatahub/GetGeoJSON?id=" + item).done(function (data) {
                map.data.addGeoJson(data);
            })
        })
    }).error(function (jqXHR, textStatus, errorThrown) {
        console.log("error " + textStatus);
        console.log("incoming Text " + jqXHR.responseText);
    });

    //map.data.setStyle(function (feature) {
    //    var low = [5, 69, 54];  // color of smallest datum
    //    var high = [151, 83, 34];   // color of largest datum

    //    var statisticdata = -1;

    //    var temp = feature.getProperty('DZ_CODE');

    //    for (var i = 0; i < data.datacode.length; i++) { 
    //        if (data.datacode[i] == temp) {
    //            statisticdata = data.data[i];
    //            break;
    //        }

    //    }

    //    var delta = (statisticdata - data.minimum) / (data.maximum - data.minimum);

    //    var color = [];

    //    for (var i = 0; i < 3; i++) {
    //        // calculate an integer color based on the delta
    //        color[i] = (high[i] - low[i]) * delta + low[i];
    //    }


    //    // determine whether to show this shape or not
    //    var showRow = true;
    //    if (statisticdata == -1) {
    //        showRow = false;
    //    }

    //    var outlineWeight = 0.5, zIndex = 1;
    //    if (feature.getProperty('datazone') === 'hover') {
    //        outlineWeight = zIndex = 2;
    //    }

    //    return {
    //        strokeWeight: outlineWeight,
    //        strokeColor: '#fff',
    //        zIndex: zIndex,
    //        fillColor: 'hsl(' + color[0] + ',' + color[1] + '%,' + color[2] + '%)',
    //        fillOpacity: 0.75,
    //        visible: showRow
    //    };
    //});

    // update and display the legend
    document.getElementById('census-min').textContent =
    data.minimum.toFixed(1) +"%";
    document.getElementById('census-max').textContent =
        data.maximum.toFixed(1) + "%";


    // set up the style rules and events for google.maps.Data
    map.data.addListener('mouseover', function (event) {
        // set the hover state so the setStyle function can change the border
        event.feature.setProperty('datazone', 'hover');
        var temp = event.feature.getProperty('DZ_CODE');
        var statisticdata = -1;
        for (var i = 0; i < data.datacode.length; i++) {
            if (data.datacode[i] == temp) {
                statisticdata = data.data[i];
                break;
            }

        }

        var percent = (statisticdata - data.minimum) /
            (data.maximum - data.minimum) * 100;

        // update the label
        document.getElementById('data-label').textContent =
            event.feature.getProperty('DZ_CODE');
        document.getElementById('data-value').textContent =
            statisticdata.toFixed(1) + '%';
        document.getElementById('data-box').style.display = 'block';
        document.getElementById('data-caret').style.display = 'block';
        document.getElementById('data-caret').style.paddingLeft = percent + '%';

    });

    map.data.addListener('mouseout', function (event) {
        event.feature.setProperty('datazone', 'normal');
    });

    map.data.addListener('click', function (kmlEvent) {
        SearchData(kmlEvent.feature.getProperty('DZ_CODE'), "ZoneCode");
    });

}


function loadData(datasetname) {
    var JSONObject = {
        "datasetname": datasetname,
    }

    $.ajax({
        type: "POST",
        url: sContextPath + "DatahubProfile/IndexDatahub/GetdataforHeatmapDatazone",
        data: JSON.stringify(JSONObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            InitMap(data)
        },
        error: function (xhr, err) {
            SetErrorMessage(xhr);
        }
    });
}

// show search result when click on map
function ShowPopupInformation(sInformation) {
    //var popupInformation = document.getElementById('popup-information');
    //popupInformation.innerHTML = sInformation;
    //$("#popup-information").show(250);
    $("#divinformationContainer").html(sInformation);
}



// call server side method via ajax
function SearchData(sCondition, sKeyname) {
    var JSONObject = {
        "keyvalue": sCondition,
        "keyname": sKeyname
    }

    $.ajax({
        type: "POST",
        url: sContextPath + "DatahubProfile/IndexDatahub/SearchByName",
        data: JSON.stringify(JSONObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            ShowPopupInfo(data);
            drawChartColumn(data);
            drawPieChart(data);
        },
        error: function (xhr, err) {
            SetErrorMessage(xhr);
        }
    });
}

function myFunctionColumn(pdata) {
    $.ajax({
        type: 'POST',
        url: sContextPath + 'DatahubProfile/IndexDatahub/GetChartDataforMap',
        data: JSON.stringify(pdata.dataSeries),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            drawChartColumn(data);
        },
        error: function (xhr, err) {
            if (xhr.readyState != 0 && xhr.status != 0) {
                alert('readyState: ' + xhr.readyState + '\nstatus: ' + xhr.status);
                alert('responseText: ' + xhr.responseText);
            }
        }
    });


}

function drawPieChart(data) {
    var name = Array();
    var y = Array();
    var dataArrayFinal = Array();
    for (i = 0; i < data.dataCategories.length; i++) {
        name[i] = data.dataCategories[i];
        y[i] = data.Schdata[i];
    }

    for (j = 0; j < name.length; j++) {
        var temp = new Array(name[j], y[j]);
        dataArrayFinal[j] = temp;
    }
    // Build the chart
    $('#divPieChartContainer').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: data.dataTitle
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            name: 'Percentage',
            colorByPoint: true,
            data: dataArrayFinal
        }]
    });
}

function drawChartColumn(data) {

    $('#divChartContainer')
            .highcharts(
                    {
                        chart: {
                            type: 'column'
                        },
                        title: {
                            text: data.dataTitle
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.dataCategories,
                            title: {
                                text: 'Destination'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: 'Percentages'
                            }
                        },
                        tooltip: {
                            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                            pointFormat: '<tr><td nowrap style="color:{series.color};padding:0">{series.name}: </td>'
                                    + '<td style="padding:0"><b>{point.y:.0f}</b></td></tr>',
                            footerFormat: '</table>',
                            shared: true,
                            useHTML: true
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0.2,
                                borderWidth: 0
                            }
                        },
                        series: [{ name: data.schoolname, data: data.Schdata }, { name: 'All Clients', data: data.Abdcitydata }],
                        credits: {
                            enabled: false
                        }
                    });
}

function ShowPopupInfo(data) {
    var sInformation = "<hr><div class='panel panel-primary text-center'> <div class='panel-heading'>";
    sInformation += "<h4 class='text-center'>" + data.dataTitle + "</h4>";
    sInformation += "</div><div class='panel-body'>";
    sInformation += "<table class='table table-bordered table-hover'>";
    sInformation += "<thead><tr><th> </th><th class='text-center'>" + data.schoolname + "</th><th class='text-center'> Glasgow City </th></tr></thead>";
    sInformation += "<tbody>";
    if (data.dataCategories.length != 0) {
        for (var i = 0; i < data.dataCategories.length; i++) {
            //sInformation += "<tr><td>" + data.dataCategories[i] + "</td><td  align='center'>" + "<input type='button' style='width: 50px; height:25px' value='" + data.Schdata[i].toFixed(2) + "'id='" + data.dataCategories[i] + "'" + "onclick='goToCreateURL(id)'/></td><td  align='center'>" + data.Abdcitydata[i].toFixed(2) + "</td><tr>";
            sInformation += "<tr><td class='text-left'>" + data.dataCategories[i] + "</td><td  align='center'> <a href= " + sContextPath + "DatahubProfile/IndexDatahub/GetListpupils?searchby=" + data.searchby + "&code=" + data.searchcode + "&dataname=" + data.dataCategories[i] + "'><button enabled class='btn btn-info btn-xs enabled'>" + data.Schdata[i].toFixed(2) + "</button></a></td><td  align='center'> <a href=" + sContextPath + "DatahubProfile/IndexDatahub/GetListpupils?searchby=school&code=100&dataname=" + data.dataCategories[i] + "'><button enabled class='btn btn-info btn-xs enabled'>" + data.Abdcitydata[i].toFixed(2) + "</button></a></td><tr>";
        }

    } else {

        sInformation += "<tr><td colspan='3' align='center'> No data available</td><tr>"
    }


    sInformation += "</tbody></table></div></div>";
    ShowPopupInformation(sInformation);

}

//function goToCreateURL(object) {
//    alert("goToCreateURL");
//    return object.href = '/DatahubProfile/IndexDatahub/GetListpupils?searchby=school&code=100&dataname=' + datasetname;
//    //?searchby=school&code=100&dataname=Pupils16
//}

function SetErrorMessage(xhr) {
    if (xhr.responseText.length > 0) {
        var sErrorMessage = JSON.parse(xhr.responseText).Message;
        alert(sErrorMessage);
    }
}

