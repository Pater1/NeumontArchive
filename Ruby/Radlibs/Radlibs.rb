require 'sinatra'

set :environment, :development
set :content_folder, Proc.new{File.join(root, "uploads")}

get '/' do
    @stories = Dir.entries(settings.content_folder)
        .select {|f| 
            !File.directory?File.basename(f, File.extname(f)).to_s
        }.map {|f|
            f.split('.')[0]
        }

    erb :index
end

get '/upload' do
    erb :upload
end
post '/upload' do
    filePath = 'uploads/' + params['file'][:filename]
    
    if File.file?(filePath)
        return "FAIL!"
    else
        File.open(filePath, "w") do |f|
            f.write(params['file'][:tempfile].read)
        end
        return "SUCCESS!"
    end
end

get '/:title' do
    @title = params[:title]
    filePath = settings.content_folder + "/" + @title + ".txt"
    File.open(filePath, 'r') do |f|
        @blanks = pull_blanks(f.read)
    end
    erb :readForm
end
post '/read' do
    @title = params[:title]
    filePath = settings.content_folder + "/" + @title + ".txt"
    blanks = params.drop(2).map{|p|
        p[1]
    }
    File.open(filePath, 'r') do |f|
       @textOut = fill_blanks(f.read, blanks)
    end
    erb :read
end

def pull_blanks(rawStory)
    return rawStory.scan(/(\[.*?\])/).flatten
end
def fill_blanks(rawStory, replacements)
    blanks = pull_blanks(rawStory)
    replacements.each_with_index do |item, index|
        rawStory = rawStory.sub(blanks[index], replacements[index])
    end
    return rawStory
end