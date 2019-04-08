puts "Welcome to the Totally (not) Awesome Number Guessing Game! TnAnGG for short.\n"

class HiScore
    def initialize
        @fileName = "hiscore.txt"
        if File.exists?(@fileName) 
            read_file
        else
            build_new
        end
    end
    def show
        puts "Top 5 Scores"
        @scores.each do |pair|
            puts "#{pair[0]} #{pair[1]}"
        end
    end
    def build_new
        @scores = [['1000', '---'], ['999', '---'], ['998', '---'], ['997', '---'], ['996', '---']];
    end
    def read_file
        begin
            file = File.open(@fileName)
            @dataRaw = file.read
            process_data
            file.close
        rescue
            build_new
        end
    end
    def write_file
        IO.write(@fileName, @scores)
    end
    def process_data
        @scores = @dataRaw.tr('[] ','').split(',').each_slice(2)
    end
    def check(score)
        if(@scores[4][0].to_i > score)
            add_name(score)
        end
    end
    def add_name(score)
        puts "You've acheived a top-5 score! May we have your autograph for the record books?"
        tmpPair = [score, gets.chomp]
        4.step(0, -1) do |i|
            puts i
            if @scores[i][0].to_i < score || i==0
                #put tmpPair into our prepaired hole, and break 
                @scores[i] = tmpPair
            else
                #move scores lower than tmpPair down as we search for where to insert
                @scores[i] = @scores[i-1]
            end
        end
        write_file
    end
end

score = HiScore.new;

cont = true
while cont
    rNum = rand(100) + 1
    tries = 0
    score.show
    puts "I just thought of a super seceret number in the range [1,100]. Can you guess it?"
    puts "(The super seceret number is definitely not #{rNum} by the way)\n"
    
    needUserIn = true
    while needUserIn
        begin
            userIn = Integer(gets.chomp)
            tries += 1
            if userIn == rNum
                needUserIn = false
                puts "You guessed my super seceret number correctly in only #{tries} attempt#{
                    if tries > 1
                        "s"
                    else
                        ""
                    end
                }! Well done."
                
                score.check(tries)

                needPlayAgain = true
                while needPlayAgain
                    puts "Do you want to play again? (y/n)"
                    userConf = gets.chomp

                    needPlayAgain = false
                    if userConf == "y" || userConf == "Y"
                        print "Awesome! "
                    elsif userConf == "n" || userConf == "N"
                        cont = false;
                        puts "Bye, thanks for playing Totally (not) Awesome Number Guessing Game! TnAnGG for short."
                    else
                        needPlayAgain = true
                    end
                end
            else
                puts "Sorry, #{userIn} is too #{
                    if userIn < rNum
                        "low"
                    else
                        "high"
                    end
                } to be the super seceret number I'm thinking of... Try giving me another number!\n"                               
            end
        rescue
            puts "That is not an integer... Try giving me another number please!"
            
        end
    end
end