package com.example.patrickconner.calculator;

public class Number extends FormulaComponent{
    public static Number Parse(String num){
        return new Number(Double.parseDouble(num));
    }
    private double number;

    public Number(double value){
        number = value;
    }

    @Override
    public Number Evaluate(Formula from) {
        return this;
    }
    public double Value() {
        return number;
    }
}
