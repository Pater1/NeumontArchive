require 'date'

class Holiday
    def initialize
        if self.class == Holiday
            raise "Sorry, the class #{self.Class} is abstract."
        end
    end

    def printHoliday
        puts "nothing"
    end
end

class FixedHoliday < Holiday
    def initialize(name, day_in_month, month_in_year, year)
        @name = name
        @day_in_month = Integer(day_in_month)
        @month = Integer(month_in_year)
        @year = Integer(year)

        quearyDayName
    end

    def quearyDayName
        date = Date.new(@year, @month, @day_in_month)
        @month_name = date.strftime("%B")
        @day_name = date.strftime("%A")
    end

    def printHoliday
        puts "#{@name} in on #{@day_name}, #{@month_name} #{@day_in_month} in #{@year}"
    end
end

class MovingHoliday < Holiday
    def initialize(name, month_in_year, day_in_week, occurence_in_month, year)
        @name = name
        @day_name = day_in_week
        @month = Integer(month_in_year)
        @year = Integer(year)
        @occurence = Integer(occurence_in_month)
        quearyDay
    end

    def quearyDay
        date = Date.new(@year, @month, 1)
        while !date.send(@day_name.downcase+"?")
            date += 1
        end
        date += 7 * (@occurence-1)

        @month_name = date.strftime("%B")
        @day_in_month = date.strftime("%d")
    end

    def printHoliday
        puts "#{@name} in on #{@day_name}, #{@month_name} #{@day_in_month} in #{@year}"
    end
end

year = 0
needYear = true
while needYear
    puts "Enter a year: "
    begin
        year = Integer(gets.chomp)
        needYear = false
    rescue
        puts "That's not a valid year! Please try again..."
    end
end

FixedHoliday.new("New Year's Day", 1, 1, year).printHoliday
FixedHoliday.new("Independence Day", 4, 7, year).printHoliday
FixedHoliday.new("Halloween", 31, 10, year).printHoliday
MovingHoliday.new("Mother's Day", 5, "Sunday", 2, year).printHoliday
MovingHoliday.new("Father's Day", 6, "Sunday", 3, year).printHoliday
MovingHoliday.new("Thanksgiving", 11, "Thursday", 4, year).printHoliday