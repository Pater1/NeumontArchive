class MorseConvert
    @@charMapEncode = {
        "A" => "·−",
        "B" => "−···",
        "C" => "−·−·",
        "D" => "−··",
        "E" => "·",
        "F" => "··−·",
        "G" => "−−·",
        "H" => "····",
        "I" => "··",
        "J" => "·−−−",
        "K" => "−·−",
        "L" => "·−··",
        "M" => "−−",
        "N" => "−·",
        "O" => "−−−",
        "P" => "·−−·",
        "Q" => "−−·−",
        "R" => "·−·",
        "S" => "···",
        "T" => "−",
        "U" => "··−",
        "V" => "···−",
        "W" => "·−−",
        "X" => "−··−",
        "Y" => "−·−−",
        "Z" => "−−··",
        
        "1" => "·−−−−",
        "2" => "··−−−",
        "3" => "···−−",
        "4" => "····−",
        "5" => "·····",
        "6" => "−····",
        "7" => "−−···",
        "8" => "−−−··",
        "9" => "−−−−·",
        "0" => "−−−−−",

        " " => " "
    }
    @@charMapDecode = {
        "·−"    => "A",
        "−···"  => "B",
        "−·−·"  => "C",
        "−··"   => "D",
        "·"     => "E",
        "··−·"  => "F",
        "−−·"   => "G",
        "····"  => "H",
        "··"    => "I",
        "·−−−"  => "J",
        "−·−"   => "K",
        "·−··"  => "L",
        "−−"    => "M",
        "−·"    => "N",
        "−−−"   => "O",
        "·−−·"  => "P",
        "−−·−"  => "Q",
        "·−·"   => "R",
        "···"   => "S",
        "−"     => "T",
        "··−"   => "U",
        "···−"  => "V",
        "·−−"   => "W",
        "−··−"  => "X",
        "−·−−"  => "Y",
        "−−··"  => "Z",
        
        "·−−−−" => "1",
        "··−−−" => "2",
        "···−−" => "3",
        "····−" => "4",
        "·····" => "5",
        "−····" => "6",
        "−−···" => "7",
        "−−−··" => "8",
        "−−−−·" => "9",
        "−−−−−" => "0",

        "  "    => " ",
        " "     => ""
    }
    def self.Encode(inputFilePath)
        #file extention not provided
        if File.extname(inputFilePath) == ""
            inputFilePath = Dir[File.basename(inputFilePath) + ".*"].first
        end

        #create output file path from input
        outputFilePath = File.join( File.dirname(inputFilePath),
                                    File.basename(inputFilePath, '.*'))
        outputFilePath += "_encode.txt"

        #create or open output file
        outputFile = nil
        if File.exists?(inputFilePath)
            outputFile = File.new(outputFilePath, 'w')
        else
            outputFile = File.open(outputFilePath)
        end

        #decode
        IO.foreach(inputFilePath) {|inputLine| 
            outputLine = ""
            inputLine.each_char do |inputChar|
                if @@charMapEncode.key?(inputChar.upcase)
                    outputChar = @@charMapEncode[inputChar.upcase] + " "
                    outputLine += outputChar
                end
            end
            outputFile.write(outputLine)
        }

        outputFile.close
    end
    def self.Decode(inputFilePath)
        #file extention not provided
        if File.extname(inputFilePath) == ""
            inputFilePath = Dir[File.basename(inputFilePath) + ".*"].first
        end

        #create output file path from input
        outputFilePath = File.join( File.dirname(inputFilePath),
                                    File.basename(inputFilePath, '.*'))
        outputFilePath += "_decode.txt"

        #create or open output file
        outputFile = nil
        if File.exists?(inputFilePath)
            outputFile = File.new(outputFilePath, 'w')
        else
            outputFile = File.open(outputFilePath)
        end

        #decode
        IO.foreach(inputFilePath) {|inputLine| 
            outputLine = ""
            inputChar = ""
            lastChar = ""
            inputLine.each_char do |inputSubChar|
                if inputSubChar != " "
                    inputChar += inputSubChar
                elsif lastChar == " "
                    outputLine += " "

                else
                    if @@charMapDecode.key?(inputChar)
                        outputChar = @@charMapDecode[inputChar]
                        outputLine += outputChar
                        inputChar = ""
                    end
                end
                lastChar = inputSubChar
            end
            outputFile.write(outputLine)
        }

        outputFile.close
    end
end

if ARGV[0].to_s.downcase == "encode"
    MorseConvert.Encode(ARGV[1])
elsif ARGV[0].to_s.downcase == "decode"
    MorseConvert.Decode(ARGV[1])
end