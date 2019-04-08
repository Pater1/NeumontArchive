package com.example.patrickconner.calculator;

import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.graphics.ColorSpace;
import android.media.MediaPlayer;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import java.io.PrintWriter;
import java.util.ArrayList;

public class MainActivity extends AppCompatActivity {
    private String formula = "";

    private final int defaultButtonColor = Color.DKGRAY;
    private final int operatorButtonColor = Color.LTGRAY;
    private final int highlightButtonColor = Color.CYAN;

    private final int defaultButtonTextColor = Color.WHITE;
    private final int operatorButtonTextColor = Color.BLACK;
    private final int highlightButtonTextColor = Color.RED;

    public void hilightButton(FadeLevel fade){
        for(int i = fades.size()-1; i >= 0; i--){
            FadeLevel f = fades.get(i);
            if(f.onBackground == fade.onBackground &&
               f.overrideText == fade.overrideText){
                fades.remove(f);
            }
        }
        fades.add(fade);
    }

    public void setFormula(String formula) {
        this.formula = formula;
        formulaDisplay.setText(formula);
        outputDisplay.setText(processFormula_String(formula));

        editor.putString("formula", formula);
        editor.apply();
    }

    private String processFormula_String(String formula) {
        try{
            return "" + Formula.Parse(formula).Evaluate().Value();
        }catch (Exception ex){
            Log.e("","", ex);
            return ex.getMessage();
        }
    }

    public String getFormula(){
        return formula;
    }

    private TextView formulaDisplay;
    private TextView outputDisplay;

    private ArrayList<FadeLevel> fades = new ArrayList<FadeLevel>();
    private SharedPreferences.Editor editor;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        final String MY_PREFS_NAME = "MyPrefsFile";
        editor = getSharedPreferences(MY_PREFS_NAME, MODE_PRIVATE).edit();

        SharedPreferences prefs = getSharedPreferences(MY_PREFS_NAME, MODE_PRIVATE);
        String restoredText = prefs.getString("formula", null);

        final MediaPlayer media = MediaPlayer.create(this, R.raw.button_press_sound_effect);
        media.start();

        final Handler handler = new Handler();
        (new Thread(){
            @Override
            public void run() {
                while (true) {
                    for (int i = fades.size() - 1; i >= 0; i--) {
                        final FadeLevel fade = fades.get(i);
                        handler.post(new Runnable() {
                            public void run() {
                                double lerp = fade.lerp - 0.01;
                                lerp = lerp < 0? 0: lerp;
                                lerp = lerp > 1? 1: lerp;
                                fade.serLerp(lerp);
                            }
                        });
                        if (fade.lerp <= 0) fades.remove(fade);
                    }
                    // next will pause the thread for some time
                    try {
                        sleep(10);
                    } catch (InterruptedException e) {
                        break;
                    }
                }
            }
        }).start();

        // row 0
        final Button button_save_number = findViewById(R.id.button_save_number);
        hilightButton(new FadeLevel(button_save_number, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_save_number, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_save_number.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                //save Number

                hilightButton(new FadeLevel(button_save_number, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_save_number, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));

                media.start();
            }
        });
        outputDisplay = findViewById(R.id.outputDisplay);
        // row 1
        final Button button_save_formula = findViewById(R.id.button_save_formula);
        hilightButton(new FadeLevel(button_save_formula, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_save_formula, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_save_formula.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                //save Number

                hilightButton(new FadeLevel(button_save_formula, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_save_formula, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));


                media.start();
            }
        });
        formulaDisplay = findViewById(R.id.formulaDisplay);
        //row 2
        final Button button_clear = findViewById(R.id.button_clear);
        hilightButton(new FadeLevel(button_clear, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_clear, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_clear.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula("");
                hilightButton(new FadeLevel(button_clear, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_clear, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button_adv = findViewById(R.id.button_adv);
        hilightButton(new FadeLevel(button_adv, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_adv, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_adv.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                //remap buttons

                hilightButton(new FadeLevel(button_adv, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_adv, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button_trig = findViewById(R.id.button_trig);
        hilightButton(new FadeLevel(button_trig, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_trig, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_trig.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                //remap buttons

                hilightButton(new FadeLevel(button_trig, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_trig, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button_back = findViewById(R.id.button_back);
        hilightButton(new FadeLevel(button_back, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_back, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_back.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                String s = getFormula();
                setFormula(s.substring(0, (s.length()-1 >= 0)? s.length()-1: 0));

                hilightButton(new FadeLevel(button_back, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_back, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));


                media.start();
            }
        });

        //row 3
        final Button button_mem = findViewById(R.id.button_mem);
        hilightButton(new FadeLevel(button_mem, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_mem, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_mem.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                //show memory

                hilightButton(new FadeLevel(button_mem, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_mem, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button_open_paren = findViewById(R.id.button_open_paren);
        hilightButton(new FadeLevel(button_open_paren, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_open_paren, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_open_paren.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "(");

                hilightButton(new FadeLevel(button_open_paren, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_open_paren, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button_close_paren = findViewById(R.id.button_close_paren);
        hilightButton(new FadeLevel(button_close_paren, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_close_paren, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_close_paren.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + ")");

                hilightButton(new FadeLevel(button_close_paren, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_close_paren, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button_mod = findViewById(R.id.button_mod);
        hilightButton(new FadeLevel(button_mod, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_mod, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_mod.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "%");

                hilightButton(new FadeLevel(button_mod, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_mod, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));


                media.start();
            }
        });

        //row 4
        final Button button1 = findViewById(R.id.button1);
        hilightButton(new FadeLevel(button1, highlightButtonColor, defaultButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button1, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));
        button1.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "1");

                hilightButton(new FadeLevel(button1, highlightButtonColor, defaultButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button1, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button2 = findViewById(R.id.button2);
        hilightButton(new FadeLevel(button2, highlightButtonColor, defaultButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button2, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));
        button2.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "2");

                hilightButton(new FadeLevel(button2, highlightButtonColor, defaultButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button2, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button3 = findViewById(R.id.button3);
        hilightButton(new FadeLevel(button3, highlightButtonColor, defaultButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button3, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));
        button3.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "3");

                hilightButton(new FadeLevel(button3, highlightButtonColor, defaultButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button3, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button_mult = findViewById(R.id.button_mult);
        hilightButton(new FadeLevel(button_mult, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_mult, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_mult.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "*");

                hilightButton(new FadeLevel(button_mult, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_mult, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));


                media.start();
            }
        });

        //row 5
        final Button button4 = findViewById(R.id.button4);
        hilightButton(new FadeLevel(button4, highlightButtonColor, defaultButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button4, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));
        button4.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "4");

                hilightButton(new FadeLevel(button4, highlightButtonColor, defaultButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button4, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button5 = findViewById(R.id.button5);
        hilightButton(new FadeLevel(button5, highlightButtonColor, defaultButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button5, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));
        button5.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "5");

                hilightButton(new FadeLevel(button5, highlightButtonColor, defaultButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button5, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button6 = findViewById(R.id.button6);
        hilightButton(new FadeLevel(button6, highlightButtonColor, defaultButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button6, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));
        button6.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "6");

                hilightButton(new FadeLevel(button6, highlightButtonColor, defaultButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button6, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button_div = findViewById(R.id.button_div);
        hilightButton(new FadeLevel(button_div, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_div, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_div.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "/");

                hilightButton(new FadeLevel(button_div, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_div, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));


                media.start();
            }
        });

        //row 6
        final Button button7 = findViewById(R.id.button7);
        hilightButton(new FadeLevel(button7, highlightButtonColor, defaultButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button7, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));
        button7.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "7");

                hilightButton(new FadeLevel(button7, highlightButtonColor, defaultButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button7, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button8 = findViewById(R.id.button8);
        hilightButton(new FadeLevel(button8, highlightButtonColor, defaultButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button8, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));
        button8.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "8");

                hilightButton(new FadeLevel(button8, highlightButtonColor, defaultButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button8, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button9 = findViewById(R.id.button9);
        hilightButton(new FadeLevel(button9, highlightButtonColor, defaultButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button9, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));
        button9.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "9");

                hilightButton(new FadeLevel(button9, highlightButtonColor, defaultButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button9, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button_add = findViewById(R.id.button_add);
        hilightButton(new FadeLevel(button_add, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_add, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_add.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "+");

                hilightButton(new FadeLevel(button_add, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_add, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));


                media.start();
            }
        });

        //row 7
        final Button button0 = findViewById(R.id.button0);
        hilightButton(new FadeLevel(button0, highlightButtonColor, defaultButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button0, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));
        button0.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "0");

                hilightButton(new FadeLevel(button0, highlightButtonColor, defaultButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button0, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button00 = findViewById(R.id.button00);
        hilightButton(new FadeLevel(button00, highlightButtonColor, defaultButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button00, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));
        button00.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "00");

                hilightButton(new FadeLevel(button00, highlightButtonColor, defaultButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button00, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button_dot = findViewById(R.id.button_dot);
        hilightButton(new FadeLevel(button_dot, highlightButtonColor, defaultButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_dot, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));
        button_dot.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + ".");

                hilightButton(new FadeLevel(button_dot, highlightButtonColor, defaultButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_dot, highlightButtonTextColor, defaultButtonTextColor, 1.0, true));


                media.start();
            }
        });
        final Button button_sub = findViewById(R.id.button_sub);
        hilightButton(new FadeLevel(button_sub, highlightButtonColor, operatorButtonColor, 1.0, false));
        hilightButton(new FadeLevel(button_sub, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));
        button_sub.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                setFormula(getFormula() + "-");

                hilightButton(new FadeLevel(button_sub, highlightButtonColor, operatorButtonColor, 1.0, false));
                hilightButton(new FadeLevel(button_sub, highlightButtonTextColor, operatorButtonTextColor, 1.0, true));


                media.start();
            }
        });


        if (restoredText != null) {
            setFormula(restoredText);
        }
    }
}

class FadeLevel{
    public Button onBackground;
    public boolean overrideText = false;
    public int fromColor, toColor;
    public int[] fromComp, toComp, lerpComp;
    public double lerp;

    public void serLerp(double toLerp){
        lerp = toLerp;
        for(int i = 0; i < fromComp.length; i++){
            double fc = (fromComp[i] * lerp);
            double lerpInv = (1-lerp);
            double tc = (toComp[i] * lerpInv);
            lerpComp[i] = (int)(fc + tc);
        }
        int color = Color.argb(lerpComp[0], lerpComp[1], lerpComp[2], lerpComp[3]);
        if(overrideText) {
            onBackground.setTextColor(color);
        }else{
            onBackground.setBackgroundColor(color);
        }
    }

    public FadeLevel(Button bg, int fr, int to, double lpstrt, boolean over){
        onBackground = bg;
        fromColor = fr;
        toColor = to;
        lerp = lpstrt;
        overrideText = over;

        fromComp = new int[4];
        fromComp[0] = Color.alpha(fromColor);
        fromComp[1] = Color.red(fromColor);
        fromComp[2] = Color.green(fromColor);
        fromComp[3] = Color.blue(fromColor);

        toComp = new int[4];
        toComp[0] = Color.alpha(toColor);
        toComp[1] = Color.red(toColor);
        toComp[2] = Color.green(toColor);
        toComp[3] = Color.blue(toColor);

        lerpComp = new int[4];
    }
}
