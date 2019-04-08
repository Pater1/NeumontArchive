$(document).ready(
    function(){
        $('#box1').hide();
        $('#box2').css({
            'background-color':'#000', 
            'color':'#fff'
        });
        
        $('#box2').on('click', function(){
            $(this).fadeOut('slow');
        });
        
        $('#box3').on('dblclick', function(){
            $('#box2').fadeIn('slow');
        });
        
        var fourBig = false;
        $('#box4').on('click', function(){
           if(fourBig){
               fourBig = false;
               $(this).width('50px');
               $(this).height('50px');
           }else{
               fourBig = true;
               $(this).width('150px');
               $(this).height('150px');
           }
        });
        
        $('#box5').on('mouseover',function(){
            $(this).html('hello');
        });
        $('#box5').on('mouseout',function(){
            $(this).html('5');
        });
        
        $('.box').on('mouseover', function(){
            $(this).css({
                'background-color':'#ff0',
                'color':'#00f'
            });
        });
        $('.box').on('mouseout', function(){
            $(this).css({
                'background-color':'#fff',
                'color':'#00f'
            }); 
        });
    }
);