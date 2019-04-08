require 'tk'
require 'tkextlib/tile'
require_relative 'MatrixMath.rb'

def toRad(degrees)
	return degrees * Math::PI / 180 
end
def sin(degrees)
	return Math.sin(toRad(degrees))
end
def cos(degrees)
	return Math.cos(toRad(degrees))
end

class Matrix
	def self.Delete(name)
		source = nil
		if !File.exists?('Saved_Matrixes.txt')
			source = File.new('Saved_Matrixes.txt', 'w')
		else
			source = File.open('Saved_Matrixes.txt')
		end
		lines = source.readlines
		source.close
		
		File.open('Saved_Matrixes.txt', 'w') do |out_file|
		  lines.each do |line|
			 out_file.puts line unless line.start_with?(name)  # <== line numbers start at 0
		  end
		end
	end
	def Save(name)
		source = nil
		if !File.exists?('Saved_Matrixes.txt')
			source = File.new('Saved_Matrixes.txt', 'w')
		else
			source = File.open('Saved_Matrixes.txt')
		end
		lines = source.readlines
		source.close
		
		File.open('Saved_Matrixes.txt', 'w') do |out_file|
		  lines.each do |line|
			 out_file.puts line unless line.start_with?(name)  # <== line numbers start at 0
		  end
		  out_file.puts name + ">" + self.StringEncode
		end
	end
	def self.Load(name)
		source = nil
		if !File.exists?('Saved_Matrixes.txt')
			source = File.new('Saved_Matrixes.txt', 'w')
		else
			source = File.open('Saved_Matrixes.txt')
		end
		lines = source.readlines
		source.close
		lines.each do |line|
			if line.start_with?(name)
				return Matrix.StringDecode(line.chomp.tap{|s| s.slice!(name+">")})
			end
		end
		return nil
	end

	def TkDisplay(parent, onChange=lambda{})
		ts = self
		
		frame = TkFrame.new(parent){
			background 'grey'
			relief 'sunken'
			borderwidth 5

			padx 5
			pady 5
		}
		@valuesFrame = TkFrame.new(frame)
		valuesFrameBuilder = lambda{
			@valuesFrame.destroy
			@valuesFrame = TkFrame.new(frame){
			   relief 'raised'
			   borderwidth 3
			}
			
			columns = @numColumns.call
			(0...columns).each do |x|
				(0...@numRows).each do |y|
					data = TkVariable.new
					data.value = ts.Value(y, x).round(7)
					ent = TkEntry.new(@valuesFrame) do
						width 10
						textvariable data
						grid('row' => y, 'column' => x)
					end
					ent.bind("Key", Proc.new do
						begin
							val = eval(ent.get).to_f
							data.value = val.round(7)
							ts.Set(y, x, val)
							onChange.call
						rescue Exception => e
						end
					end)
				end
			end
			
			@valuesFrame.grid(	'row' => 1, 
						'column' => 0, 
						'padx' => 0, 
						'pady' => 0,
						'ipadx' => 0, 
						'ipady' => 0)
		}
		valuesFrameBuilder.call
		
		@sizeFrame = nil
		sizeFrameBuilder = lambda{
			@sizeFrame = TkFrame.new(frame){
			   relief 'raised'
			   borderwidth 3
			}
					
			data = TkVariable.new
			data.value = @numRows
			ent = TkEntry.new(@sizeFrame) do
				width 10
				textvariable data
				grid('row' => 0, 'column' => 0)
			end
			ent.bind("Key", Proc.new do
					begin
					cols = ts.numColumns.call
					rows = eval(ent.get).to_f
					
					ts.numRows = rows
					ts.values((1..(cols * rows)).to_a)
					
					valuesFrameBuilder.call
					
					onChange.call
				rescue SyntaxError => e
				rescue StandardError => x
				end
			end)
			 
			TkLabel.new(@sizeFrame) do
				width 5
				text "X"
				grid('row' => 0, 'column' => 1)
			end
					
			data = TkVariable.new
			data.value = @numColumns.call
			try = TkEntry.new(@sizeFrame) do
				width 10
				textvariable data
				grid('row' => 0, 'column' => 2)
			end
			try.bind("Key", Proc.new do
				begin
					rows = ts.numRows
					cols = eval(try.get).to_f
					
					ts.numRows = rows
					ts.values((1..(cols * rows)).to_a)
					
					valuesFrameBuilder.call
					
					onChange.call
				rescue SyntaxError => e
				rescue StandardError => x
				end
			end)
			
			
			@sizeFrame.grid(	'row' => 0, 
							'column' => 0, 
							'padx' => 0, 
							'pady' => 5,
							'ipadx' => 0, 
							'ipady' => 0)
		}
		sizeFrameBuilder.call
		
		saveFrame = TkFrame.new(frame){
		   relief 'raised'
		   borderwidth 3
		}
		data = TkVariable.new
		data.value = "A"
		sv = TkEntry.new(saveFrame) do
			width 10
			textvariable data
			grid('row' => 0, 'column' => 1)
		end		
		TkButton.new(saveFrame)do
			text "Save this Matrix!"
			command do
				ts.Save(sv.get)
			end
			grid('row' => 0, 'column' => 2)
		end
		TkButton.new(saveFrame)do
			text "Load this Matrix!"
			command do
				tmp = Matrix.Load(sv.get)
				if tmp != nil
					ts.numRows = tmp.numRows
					ts.values = tmp.values
					
					valuesFrameBuilder.call
					sizeFrameBuilder.call
				end
			end
			grid('row' => 0, 'column' => 3)
		end
		
		saveFrame.grid(	'row' => 2, 'column' => 0)
		
		if block_given?
			yield frame
		end
		
		return frame
	end
	
	def TkDisplay_readOnly(parent, savedName=nil, onDelete=nil)
		ts = self
		
		frame = TkFrame.new(parent){
			background 'grey'
			relief 'sunken'
			borderwidth 5

			padx 5
			pady 5
		}
		@valuesFrame = TkFrame.new(frame)
		valuesFrameBuilder = lambda {
			@valuesFrame.destroy
			@valuesFrame = TkFrame.new(frame){
			   relief 'raised'
			   borderwidth 3
			}
			
			columns = @numColumns.call
			(0...@numRows).each do |x|
				(0...columns).each do |y|
					TkLabel.new(@valuesFrame) do
						width 10
						text ts.Value(x, y).round(3)
						grid('row' => x, 'column' => y)
					end
				end
			end
			
			@valuesFrame.grid(	'row' => 2, 
						'column' => 0, 
						'padx' => 0, 
						'pady' => 0,
						'ipadx' => 0, 
						'ipady' => 0)
		}
		valuesFrameBuilder.call
		
		@sizeFrame = nil
		sizeFrameBuilder = lambda{
			@sizeFrame = TkFrame.new(frame){
			   relief 'raised'
			   borderwidth 3
			}
			
			rows = @numRows
			ent = TkLabel.new(@sizeFrame) do
				width 10
				text rows
				grid('row' => 0, 'column' => 0)
			end
			TkLabel.new(@sizeFrame) do
				width 5
				text "X"
				grid('row' => 0, 'column' => 1)
			end
			cols = @numColumns.call
			try = TkLabel.new(@sizeFrame) do
				width 10
				text cols
				grid('row' => 0, 'column' => 2)
			end
			
			@sizeFrame.grid(	'row' => 1, 
							'column' => 0, 
							'padx' => 0, 
							'pady' => 5,
							'ipadx' => 0, 
							'ipady' => 0)
		}
		sizeFrameBuilder.call
		
		saveFrame = TkFrame.new(frame){
		   relief 'raised'
		   borderwidth 3
		}
		if savedName == nil
			data = TkVariable.new
			data.value = "A"
			sv = TkEntry.new(saveFrame) do
				width 10
				textvariable data
				grid('row' => 0, 'column' => 1)
			end		
			TkButton.new(saveFrame)do
				text "Save this Matrix!"
				command do
					ts.Save(sv.get)
				end
				grid('row' => 0, 'column' => 2)
			end
			saveFrame.grid(	'row' => 3, 'column' => 0)
		else
			TkLabel.new(saveFrame) do
				width 10
				text savedName
				grid('row' => 0, 'column' => 1)
			end	
			TkButton.new(saveFrame)do
				text "Delete this Matrix!"
				command do
					Matrix.Delete(savedName)
					if onDelete != nil
						onDelete.call
					end
				end
				grid('row' => 0, 'column' => 2)
			end
			saveFrame.grid(	'row' => 0, 'column' => 0)
		end
		
		
		if block_given?
			yield frame
		end
		 
		return frame
	end
end

def Draw(parent)
	root = TkFrame.new(parent){
		relief 'flat'
		borderwidth 5

		padx 5
		pady 5
	}
	
	if block_given?
		yield root
	end
	return root
end

def DrawMatrixMult(parent)
	root = TkFrame.new(parent){
		relief 'flat'
		borderwidth 5

		padx 5
		pady 5
	}

	m = Matrix.new(3, [1,2,4,5,7,8])
	s = m.StringEncode
	i = Matrix.StringDecode(s)
	ret = TkLabel.new
	callback = lambda{
		ret.destroy
		begin
			ret = m.Multiply(i).TkDisplay_readOnly(root) do |x|
				x.grid(	'row' => 0, 
						'column' => 4, 
						'padx' => 0, 
						'pady' => 5,
						'ipadx' => 0, 
						'ipady' => 0)
			end
		rescue
			ret = TkLabel.new(root) do 
				text "Incompatable Matrixes"
				grid(	'row' => 0, 
						'column' => 4, 
						'padx' => 0, 
						'pady' => 5,
						'ipadx' => 0, 
						'ipady' => 0)
			end
		end
	}

	m.TkDisplay(root, callback) do |x|
	x.grid(	'row' => 0, 
			'column' => 0, 
			'padx' => 0, 
			'pady' => 5,
			'ipadx' => 0, 
			'ipady' => 0)
	end
	TkLabel.new(root) do
		width 5
		text "X"
		grid('row' => 0, 'column' => 1)
	end
	i.TkDisplay(root, callback) do |x|
		x.grid(	'row' => 0, 
				'column' => 2, 
				'padx' => 0, 
				'pady' => 5,
				'ipadx' => 0, 
				'ipady' => 0)
	end
	TkLabel.new(root) do
				width 5
				text "="
				grid('row' => 0, 'column' => 3)
			end
	
	callback.call
	if block_given?
		yield root
	end
	return root
end

def DrawMatrixStats(parent)
	root = TkFrame.new(parent){
		relief 'flat'
		borderwidth 5

		padx 5
		pady 5
	}
	
	k = Matrix.new(2, [1,2,3,4])
	det = TkLabel.new(root) 
	inv = TkLabel.new(root) 
	cl = lambda{
		det.destroy
		inv.destroy
		begin
			det = TkLabel.new(root) do 
				text "det = #{k.Determinant.to_s}"
				grid(	'row' => 0, 
						'column' => 1, 
						'padx' => 0, 
						'pady' => 5,
						'ipadx' => 0, 
						'ipady' => 0)
			end
			inv = k.Inverse.TkDisplay_readOnly(root) do |x|
				x.grid(	'row' => 0, 
						'column' => 2, 
						'padx' => 0, 
						'pady' => 5,
						'ipadx' => 0, 
						'ipady' => 0)
			end
		rescue
			det = TkLabel.new do 
				text "Non-square Matrix"
				grid(	'row' => 0, 
						'column' => 1,
						'columnspan' => 2, 
						'padx' => 0, 
						'pady' => 5,
						'ipadx' => 0, 
						'ipady' => 0)
			end
		end
	}
	k.TkDisplay(root, cl) do |x|
		x.grid(	'row' => 0, 
				'column' => 0, 
				'padx' => 0, 
				'pady' => 5,
				'ipadx' => 0, 
				'ipady' => 0)
	end
	
	cl.call
	
	if block_given?
		yield root
	end
	return root
end

def DrawMatrixRowOps(parent)
	root = TkFrame.new(parent){
		relief 'flat'
		borderwidth 5

		padx 5
		pady 5
	}
	l = Matrix.new(2, [1,2,3,4])
	ldis = l.TkDisplay(root) do |x|
		x.grid(	'row' => 0, 
				'column' => 0, 
				'padx' => 0, 
				'pady' => 5,
				'ipadx' => 0, 
				'ipady' => 0)
	end
	
	rowOpsFrame = TkFrame.new(root){
				relief 'sunken'
				borderwidth 5

				padx 5
				pady 5
			}
	TkLabel.new(rowOpsFrame) do
		width 5
		text "Row("
		grid('row' => 2, 'column' => 1)
	end
	d = TkVariable.new
	r1 = TkEntry.new(rowOpsFrame) do
		width 10
		textvariable d
		grid('row' => 2, 'column' => 2)
	end
	TkLabel.new(rowOpsFrame) do
		width 5
		text ") += "
		grid('row' => 2, 'column' => 3)
	end
	d = TkVariable.new
	sc = TkEntry.new(rowOpsFrame) do
		width 5
		textvariable d
		grid('row' => 2, 'column' => 4)
	end
	TkLabel.new(rowOpsFrame) do
		width 5
		text " * Row("
		grid('row' => 2, 'column' => 5)
	end
	d = TkVariable.new
	r2 = TkEntry.new(rowOpsFrame) do
		width 5
		textvariable d
		grid('row' => 2, 'column' => 6)
	end
	TkLabel.new(rowOpsFrame) do
		width 5
		text ")"
		grid('row' => 2, 'column' => 7)
	end
	TkButton.new(rowOpsFrame) do
		text "Scale and Add!"
		command do
			begin
				l = l.RowAdd(eval(r1.get).to_i, eval(r2.get).to_i, eval(sc.get).to_f)
				ldis.destroy
				ldis = l.TkDisplay(root) do |x|
					x.grid(	'row' =>0, 
							'column' => 0, 
							'padx' => 0, 
							'pady' => 5,
							'ipadx' => 0, 
							'ipady' => 0)
				end
			rescue SyntaxError => e
			rescue StandardError => x
			end
		end
		grid('row' => 2, 'column' => 8)
	end


	TkLabel.new(rowOpsFrame) do
		width 5
		text "Row("
		grid('row' => 3, 'column' => 1)
	end
	d = TkVariable.new
	r3 = TkEntry.new(rowOpsFrame) do
		width 10
		textvariable d
		grid('row' => 3, 'column' => 2)
	end
	TkLabel.new(rowOpsFrame) do
		width 5
		text ") *= "
		grid('row' => 3, 'column' => 3)
	end
	d = TkVariable.new
	al = TkEntry.new(rowOpsFrame) do
		width 5
		textvariable d
		grid('row' => 3, 'column' => 4)
	end
	TkButton.new(rowOpsFrame) do
		text "Multiply!"
		command do
			begin
				l = l.RowMult(eval(r3.get).to_i, eval(al.get).to_f)
				ldis.destroy
				ldis = l.TkDisplay(root) do |x|
					x.grid(	'row' => 0, 
							'column' => 0, 
							'padx' => 0, 
							'pady' => 5,
							'ipadx' => 0, 
							'ipady' => 0)
				end
			rescue SyntaxError => e
			rescue StandardError => x
			end
		end
		grid('row' => 3, 'column' => 8)
	end

	TkLabel.new(rowOpsFrame) do
		width 5
		text "Row("
		grid('row' => 4, 'column' => 1)
	end
	d = TkVariable.new
	r4 = TkEntry.new(rowOpsFrame) do
		width 10
		textvariable d
		grid('row' => 4, 'column' => 2)
	end
	TkLabel.new(rowOpsFrame) do
		width 15
		text ") swaps with Row("
		grid('row' => 4, 'column' => 3)
	end
	d = TkVariable.new
	r5 = TkEntry.new(rowOpsFrame) do
		width 5
		textvariable d
		grid('row' => 4, 'column' => 4)
	end
	TkLabel.new(rowOpsFrame) do
		width 5
		text ")"
		grid('row' => 4, 'column' => 5)
	end
	TkButton.new(rowOpsFrame) do
		text "Swap Rows!"
		command do
			begin
				l = l.RowSwap(eval(r4.get).to_i, eval(r5.get).to_i)
				ldis.destroy
				ldis = l.TkDisplay(root) do |x|
					x.grid(	'row' => 0, 
							'column' => 0, 
							'padx' => 0, 
							'pady' => 5,
							'ipadx' => 0, 
							'ipady' => 0)
				end
			rescue SyntaxError => e
			rescue StandardError => x
			end
		end
		grid('row' => 4, 'column' => 8)
	end
	rowOpsFrame.grid('row' => 0, 'column' => 1, 'columnspan' => 5)
	
	if block_given?
		yield root
	end
	return root
end

$matrixes = []
def DrawSavedMatrixes(parent)
	root = TkFrame.new(parent){
		relief 'flat'
		borderwidth 5

		padx 5
		pady 5
	}
	
	build = lambda{
		$matrixes.each{|x| x.destroy}
		
		source = nil
		if !File.exists?('Saved_Matrixes.txt')
			source = File.new('Saved_Matrixes.txt', 'w')
		else
			source = File.open('Saved_Matrixes.txt')
		end
		lines = source.readlines
		source.close
		lines.each do |line|
			$matrixes << Matrix.StringDecode(line.chomp.split(">")[1]).TkDisplay_readOnly(root, line.chomp.split(">")[0], build) {|x| x.pack}
		end
	}
	TkButton.new(root)do
		text "Refresh"
		command build
		pack
	end
	TkButton.new(root)do
		text "Delete All"
		command do
			source = nil
			if !File.exists?('Saved_Matrixes.txt')
				source = File.new('Saved_Matrixes.txt', 'w')
			else
				source = File.open('Saved_Matrixes.txt')
			end
			source.close
			
			File.open('Saved_Matrixes.txt', 'w') do |out_file|
			end
			
			build.call
		end
		pack
	end
	
	build.call
	
	if block_given?
		yield root
	end
	return root
end

def DrawMenu(parent)
	root = Tk::Tile::Notebook.new(parent)do
	   grid('row' => 2, 'column' => 0)
	end

	root.add DrawSavedMatrixes(root)	, :text => 'Saved Matrixes'
	root.add DrawMatrixRowOps(root)	, :text => 'Matrix Row Operations'
	root.add DrawMatrixStats(root)	, :text => 'Matrix Stats'
	root.add DrawMatrixMult(root)	, :text => 'Matrix Multiply'
	root.add DrawMatrixMult(root)	, :text => 'Transformation Matrixes', :state =>'disabled'
	root.add DrawMatrixMult(root)	, :text => 'Vector Operations', :state =>'disabled'
	root.add DrawMatrixMult(root)	, :text => 'Eigen Analysis', :state =>'disabled'
	
	if block_given?
		yield root
	end
	return root
end

base = TkRoot.new { title "Matrix Calculator" }

DrawMenu(base) do |x|
	x.grid(	'row' => 0, 
			'column' => 0, 
			'padx' => 0, 
			'pady' => 5,
			'ipadx' => 0, 
			'ipady' => 0)
end

Tk.mainloop