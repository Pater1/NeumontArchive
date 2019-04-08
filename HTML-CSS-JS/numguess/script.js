var rand, needRefresh = true, guessCount;
var feedback;

function validateGuess(toValidate) {
    "use strict";
    if (toValidate === rand) {
        needRefresh = true;
        feedback.innerHTML = "Got it! " + ((guessCount > 10)? "But, it took you " + guessCount + " tries...": "And, it only took you " + guessCount + " tr"+((guessCount == 1)? "y" : "ies")+"!<br/> <a href = 'https://www.youtube.com/watch?v=FF5C04CB-VQ' target='_blank'><img src = 'https://i.ytimg.com/vi/ugAZHa80Ujk/maxresdefault.jpg'/></a>");
        
        document.getElementById("body").backgroundColor = "#f00";
    } else {
        feedback.innerHTML = "" + toValidate + ((toValidate > rand) ? " is too much!" : " is too little!") + " " + guessCount + " guesses so far.";
    }
}

function updateRandom() {
    "use strict";
    rand = Math.floor(Math.random() * 100);
    needRefresh = false;
    console.log(rand);
    guessCount = 0;
}

function sendData(form) {
    "use strict";
    feedback = document.getElementById("feedback");body
    
    document.getElementById("body").backgroundColor = "#fff";
    
    var guessNum = Math.floor(form.inputbox.value);
    form.inputbox.value = "";
    
    if(isNaN(guessNum)){
        feedback.innerHTML = "Numbers only please!";
        return;
    }
    if(guessNum > 100 || guessNum < 0){
        feedback.innerHTML = "Restrict your numbers between 0 and 100 please...";
        return;
    }
    
    guessCount ++;
    
    validateGuess(guessNum);
    
    if (needRefresh) {
        updateRandom();
    }
}

updateRandom();