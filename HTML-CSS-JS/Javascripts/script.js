
var a = ["hi", "heh", "hello", "words", "<b>", "</b>", "<em>", "</em>", "<q>", "</q>", "<br/>", "<div>", "</div>"];

var str = "";
for (var i = 0; i < 10000; i++){
    str += randSelect(a);   
}

document.getElementById('thing').innerHTML = str;

function randSelect(array){
    return array[Math.floor(Math.random() * (array.length))];
}