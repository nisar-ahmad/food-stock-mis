var items = [];
var commodity;
var bags;
var quantity;
var rate;
var amount;

function calculateQuantity() {
    var id = commodity.val();
    if (id)
        quantity.val(bags.val() * items[id].kg);
}

function bagsChanged() {
    calculateQuantity();
    calculateAmount();
}

function rateChanged() {
    calculateAmount();
}

function calculateAmount() {
    amount.val(bags.val() * rate.val());
}

function calculateRate() {

    if (rate) {
        var id = commodity.val();
        if (id) {
            rate.val(items[id].rate);
            calculateAmount();
        }
    }
}

function commodityChanged() {
    calculateQuantity();
    calculateRate();
}

function init(getUrl, commodityid, bagsid, quanityid, rateid, amountid) {

    if (!commodityid)
        commodityid = "#CommodityTypeId";

    if (!bagsid)
        bagsid = "#Bags";

    if (!quanityid)
        quanityid = "#Quantity";

    if (!rateid)
        rateid = "#Rate";

    if (!amountid)
        amountid = "#Amount";

    commodity = $(commodityid);
    bags = $(bagsid);
    quantity = $(quanityid);
    rate = $(rateid);
    amount = $(amountid);

    commodity.attr("onchange", "commodityChanged()");
    bags.attr("onchange", "bagsChanged()");
    bags.attr("onkeyup", "bagsChanged()");

    if (rate)
    {
        rate.attr("onchange", "rateChanged()");
        rate.attr("onkeyup", "rateChanged()");
    }
    
    $.getJSON(getUrl, function (data) {
        $.each(data, function (i, item) {
            items[item.id] = { kg: item.kg, rate: item.rate }
        });

        if(rate && (rate.val() == null || rate.val() == ""))
            commodityChanged();
    });
}