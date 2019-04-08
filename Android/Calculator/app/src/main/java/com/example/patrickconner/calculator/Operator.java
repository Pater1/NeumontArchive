package com.example.patrickconner.calculator;

import java.util.ArrayList;

public class Operator extends FormulaComponent{
    public char operator;
    public Operator(char operator){
        this.operator = operator;
    }

    @Override
    public Number Evaluate(Formula from) throws Exception {
        double valueOut = 0;
        int[] consume;
        switch (operator){
            case '+':
            case '-':
            case '*':
            case '/':
            case '%':
                consume = new int[2];
                consume[0] = -1;
                consume[1] = 1;
                break;
            default:
                throw new Exception("Can not evaluate " + operator + " operator.");
        }

        FormulaComponent[] comps = from.Consume(consume, this);
        double[] operands = new double[comps.length];
        for(int i = 0; i < comps.length; i++){
            operands[i] = comps[i] != null? comps[i].Evaluate(from).Value(): 0.0;
        }

        switch (operator){
            case '+':
                for (double d: operands) {
                    valueOut += d;
                }
                break;
            case '-':
                valueOut = operands[0];
                for(int i = 1; i < operands.length; i++){
                    valueOut -= operands[i];
                }
                break;
            case '*':
                valueOut = operands[0];
                for(int i = 1; i < operands.length; i++){
                    valueOut *= operands[i];
                }
                break;
            case '/':
                valueOut = operands[0];
                for(int i = 1; i < operands.length; i++){
                    valueOut /= operands[i];
                }
                break;
            case '%':
                valueOut = operands[0];
                for(int i = 1; i < operands.length; i++){
                    valueOut %= operands[i];
                }
                break;
            default:
                throw new Exception("Can not evaluate " + operator + " operator.");
        }

        return new Number(valueOut);
    }
}
