def syntheticDivision(polynomial, divisor) 
  polynomial.map! { |n| n.to_f } 
  result = polynomial.first 
  quotient = [result] 
  polynomial[1, polynomial.size - 1].each do |coefficient| 
    result *= divisor 
    result += coefficient 
    quotient << result 
  end 
  remainder = quotient.pop 
  { :quotient => quotient, :remainder => remainder } 
end 
 
class Matrix
    #indexes row-first
    #[1, 4, 7]
    #[2, 5, 8]
    #[3, 6, 9]
    attr_accessor :numRows
    attr_accessor :numColumns
    attr_accessor :values
    attr_accessor :rows
    attr_accessor :columns
	
	def values(setTo = nil)
		if setTo != nil
			@values = setTo.map{|x|
				x.to_f
			}
		end
		return @values
	end 

    def Index(y, x)
		ret = (@numColumns.call * y) + x
		
		raise ArgumentError, 'Arguments are out of range' unless (  ((x.is_a? Integer) && (y.is_a? Integer)) &&
                                                                    ((x < @numColumns.call) && (x >= 0) &&
                                                                     (y < @numRows) && (y >= 0)
                                                                    )
                                                                 )
																 
		
        return ret
    end
    def Value(y, x)
        return @values[Index(y, x)]
    end
    def Set(y, x, set)
        @values[Index(y, x)] = set.to_f
    end
	def Cofactor(x, y)
		return (x % 2 == y % 2)? 1: -1
	end
	
    def initialize(rowNumber, valuesArray)
        raise ArgumentError, 'Argument is not numeric' unless rowNumber.is_a? Numeric

        @numRows = rowNumber
        @values = valuesArray.map{|v|
			v.to_f
		}
        
        @numColumns = lambda {
			if @numRows > 0
				return @values.length / @numRows
			else 
				return 0
			end
        }
        @rows = Enumerator.new do |x|
            (0...@numRows).each do |i|
			
			end
        end
        @columns = Enumerator.new do |x|
			(0...@numColumns.call).each do |i|
			
			end
        end
    end
    def self.FromRows(rowsArray)
        return Matrix.new(rowsArray.length, rowsArray.flatten)
    end
    def self.FromColumns(columnsArray)
        return Matrix.FromRows(columnsArray).Transpose
    end

    def Transpose 
        rows = []
		(0...@numColumns.call).each do |x|
			row = []
			(0...@numRows).each do |y|
                row.push(Value(y, x))
            end
            rows.push(row)
        end
        return Matrix.FromRows(rows)
    end

    def Scale(scaler)
        raise ArgumentError, 'Argument is not numeric' unless scaler.is_a? Numeric  
		
		return Matrix.new(@numRows,  @values.map{|x| scaler * x }.to_a)
    end
    def Multiply(matrix)
        raise ArgumentError, 'Matrices are not of compatable sizes!' unless ((matrix.is_a? Matrix) && matrix.numRows == @numColumns.call)

        rows = []
        (0...@numRows).each do |y|
            row = []
            (0...matrix.numColumns.call).each do |x|
                sum = 0
                    (0...matrix.numRows).each do |c|
						a = 		Value(y,c)
						b = matrix.	Value(c,x)
                        sum += a * b
                    end
                row.push(sum)
                end
                rows.push(row)
            end
        return Matrix.FromRows(rows)
    end
    def Add(matrix)
        raise ArgumentError, 'Matrices are not the same size!' unless ((matrix.is_a? Matrix) && matrix.numRows == @numRows && matrix.numColumns.call == @numColumns.call)

        return Matrix.new(@numRows, (0...@values.length).map{|v|
            @values[v] + matrix.values[v]
        })
    end
    def Add!(matrix)
        raise ArgumentError, 'Matrices are not the same size!' unless ((matrix.is_a? Matrix) && matrix.numRows == @numRows && matrix.numColumns.call == @numColumns.call)

        return @values = (0...@values.length).map{|v|
            @values[v] + matrix.values[v]
        }
    end
    def Subtract(matrix)
        return Matrix.new(@numRows, (0...@values.length).map{|v|
            @values[v] - matrix.values[v]
        })
    end
    def Subtract!(matrix)
        raise ArgumentError, 'Matrices are not the same size!' unless ((matrix.is_a? Matrix) && matrix.numRows == @numRows && matrix.numColumns.call == @numColumns.call)

        return @values = (0...@values.length).map{|v|
            @values[v] - matrix.values[v]
        }
    end

	def RowAdd(mutatedRowIndex, pivotRowIndex, pivotScaler = 1)
		copy = Matrix.new(@numRows, @values)
		(0...copy.numColumns.call).each do |x|
			val = Value(mutatedRowIndex, x) + (Value(pivotRowIndex, x) * pivotScaler)
			copy.Set(mutatedRowIndex, x, val)
		end
		return copy
	end
	def RowMult(mutatedRowIndex, scaler)
		copy = Matrix.new(@numRows, @values)
		(0...copy.numColumns.call).each do |x|
			val =  Value(mutatedRowIndex, x) * scaler
			copy.Set(mutatedRowIndex, x, val)
		end
		return copy
	end
	def RowSwap(a, b)
		copy = Matrix.new(@numRows, @values)
		(0...copy.numColumns.call).each do |x|
			av =  Value(a, x)
			copy.Set(a, x, Value(b, x))
			copy.Set(b, x, av)
		end
		return copy
	end
	
	def SubMatrix(xCancel, yCancel)
		rows = []
        for y in (0...@numColumns.call)
			if y != yCancel
				row = []
				for x in (0...@numRows)
					if x != xCancel
						row.push(Value(y, x))
					end
				end
				rows.push(row)
			end
        end
        return Matrix.FromRows(rows)
	end
	def Determinant
		raise ArgumentError, 'Matrix is not square!' unless ((@numRows == @numColumns.call) && (@numRows > 1))
		
		if @numRows == 2
			return Value(0,0) * Value(1,1) - Value(0,1) * Value(1,0)
		else
			ret = 0
			(0...@numRows).each do |i|
				ret += Value(0,i) * SubMatrix(0,i).Determinant * Cofactor(0, i)
			end
			return ret
		end
	end
	def Inverse
		det = self.Determinant
		raise ArgumentError, 'Matrix is singular!' unless det != 0
		
		if @numRows == 2
			ret = Matrix.new(self.numRows, self.values)
			ret.Set(0,0, self.Value(1,1))
			ret.Set(1,1, self.Value(0,0))
			ret.Set(1,0, -self.Value(1,0))
			ret.Set(0,1, -self.Value(0,1))
			return ret.Scale(1.0/det)
		else
			rows = []
			for y in 0...@numColumns.call
				row = []
				for x in 0...@numRows
					row.push(
						SubMatrix(x, y).Determinant *
						Cofactor(x,y)
					)
				end
				rows.push(row)
			end
			return Matrix.FromRows(rows).Transpose.Scale(1.0/det)
		end
	end
	
	def EigenValues
		#|a-y b c|
		#|d e-y f|
		#|g h i-y|
		
		#((a-y)*(((e-y)*(i-y))-h*f)) +
		#(-1 * b * ((d*(i-y))-g*f)) + 
		#(c * (d*h-g*(e-y)))
	end
	
    def to_str 
        # ret = ""

        # columns = @numColumns.call
        # for x in (0...@numRows)
            # ret = ret + "["
            # for y in (0...columns)
                # ret = ret + Value(y, x).to_s
                # if y < (columns-1)
                    # ret = ret + ","
                # end
            # end
            # ret = ret + "]\n"
        # end
        
        # return ret;
    end
	
	def StringEncode
		ret = "#{@numRows},"
		@values.each do |x|
			ret << x.to_s << ","
		end
		return ret.chomp(',')
	end
	def self.StringDecode(string)
		vals = string.split(',').map{|x| x.to_f}
		rs = vals[0].to_i
		vs = vals.drop(1)
		return Matrix.new(rs, vs)
	end
end

class Vector
    #row vectors
    @backingMatrix
	def initialize(valuesArray)

	end
end