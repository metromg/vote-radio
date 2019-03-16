#!/usr/bin/liquidsoap
set("log.file.path", "/var/log/liquidsoap/logs.log")

def next_song() =
        result = get_process_output("python /etc/liquidsoap/next_song.py --dest {{BACKEND_HOST}} --port {{BACKEND_PORT}}")
        request.create(result)
end

offline = blank()

source = request.dynamic(next_song, length=6.0, conservative=false)
source = amplify(0.7, override="replay_gain", audio_to_stereo(source))
source = smart_crossfade(source)
source = rewrite_metadata([("title", '$(if $(use_suffix),"$(title) $(suffix)","$(title)")'), ("url", "localhost")], source)
source = fallback([source, offline], track_sensitive=false)

output.icecast( %mp3.vbr(quality=8), host="{{ICECAST_HOST}}", port={{ICECAST_PORT}}, password="{{ICECAST_SOURCE_PASSWORD}}", mount="radio.mp3", genre="Music", source)