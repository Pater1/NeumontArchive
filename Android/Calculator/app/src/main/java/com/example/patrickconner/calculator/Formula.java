package com.example.patrickconner.calculator;

import java.util.ArrayList;

public class Formula extends FormulaComponent{
    private static final char[] Operators = "+-*/%".toCharArray();
    private static final OperatorPreference[] PEMDAS = new OperatorPreference[Operators.length];
    static { // static initializer  block.
        PEMDAS[0] = new OperatorPreference(Operators[0],1);
        PEMDAS[1] = new OperatorPreference(Operators[1],1);
        PEMDAS[2] = new OperatorPreference(Operators[2],2);
        PEMDAS[3] = new OperatorPreference(Operators[3],2);
        PEMDAS[4] = new OperatorPreference(Operators[4],2);
    }
    public static Formula Parse(String s){
        ArrayList<FormulaComponent> nodes = new ArrayList<FormulaComponent>();

        String numberRegister = "";

        String subFormulaRegister = "";
        int subFormula_ParenCount = 0;
        ArrayList<FormulaComponent> toAdd = new ArrayList<FormulaComponent>();
        for(char c : s.toCharArray()){
            if(c == '('){
                subFormula_ParenCount++;
            }else if(c == ')'){
                subFormula_ParenCount--;
                if(subFormula_ParenCount == 0){
                    toAdd.add(Formula.Parse(subFormulaRegister));
                    subFormulaRegister = "";
                }
            }else if(subFormula_ParenCount > 0){
                subFormulaRegister += c;
            } else {
                if (Character.isDigit(c) || c == '.') {
                    numberRegister += c;
                } else {
                    if (numberRegister != "") {
                        toAdd.add(Number.Parse(numberRegister));
                        numberRegister = "";
                    }
                }
                if(ArrayContains(Operators, c)){
                    toAdd.add(new Operator(c));
                }
            }
        }

        if (numberRegister != "") {
            toAdd.add(Number.Parse(numberRegister));
            numberRegister = "";
        }
        if(subFormulaRegister != ""){
            toAdd.add(Formula.Parse(subFormulaRegister));
            subFormulaRegister = "";
        }

        for(int i = 0; i < toAdd.size(); i++){
            FormulaComponent adding = toAdd.get(i);
            if(nodes.size() > 0 && !(adding instanceof  Operator) && !(nodes.get(nodes.size()-1) instanceof  Operator)){
                nodes.add(new Operator('*'));
            }
            nodes.add(adding);
        }

        return new Formula(nodes);
    }

    private ArrayList<FormulaComponent> nodes;
    public Formula(ArrayList<FormulaComponent> nodes){
        this.nodes = nodes;
    }

    private static boolean ArrayContains(char[] operators, char c) {
        for(char o: operators){
            if(c == o){
                return true;
            }
        }
        return false;
    }


    public Number Evaluate() throws Exception {
        return Evaluate(this);
    }

    public FormulaComponent[] Consume(int[] relativeIndexes, FormulaComponent relativeTo){
        FormulaComponent[] ret = new FormulaComponent[relativeIndexes.length];
        for(int i = 0; i < relativeIndexes.length; i++){
            int absoluteIndex = relativeIndexes[i] + nodes.indexOf(relativeTo);
            ret[i] = (absoluteIndex >= 0 && absoluteIndex < nodes.size())? nodes.get(absoluteIndex): null;
            nodes.remove(absoluteIndex);
        }
        return ret;
    }

    @Override
    public Number Evaluate(Formula from) throws Exception {
        //Formula - 5
        //Operator - 1/2
        //Number - 0
        FormulaComponent nextOperateOn = null;
        int nextOperateImportance;
        while(nodes.size() > 1){
            nextOperateImportance = -1;
            for(FormulaComponent f: nodes){
                int thisImportance = -1;
                if(f instanceof Number) thisImportance = 0;
                else if(f instanceof Formula) thisImportance = 5;
                else if(f instanceof Operator){
                    char o = ((Operator)f).operator;
                    for(OperatorPreference op : PEMDAS){
                        if(o == op.operator){
                            thisImportance = op.priority;
                            break;
                        }
                    }
                }

                if(thisImportance > nextOperateImportance){
                    nextOperateImportance = thisImportance;
                    nextOperateOn = f;
                }
            }

            Number ret = nextOperateOn.Evaluate(this);
            int operateOnIndex = nodes.indexOf(nextOperateOn);
            nodes.set(operateOnIndex, ret);
        }

        return nodes.get(0).Evaluate(this);
    }
}

class OperatorPreference{
    public char operator;
    public int priority;

    public OperatorPreference(char o, int p){
        operator = o;
        priority = p;
    }
}