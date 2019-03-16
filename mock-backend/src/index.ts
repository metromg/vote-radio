import { watch } from 'chokidar';
import * as http from 'http';

const songs: string[] = [];

const rootPath = '/home/node/watch/';
const watcher = watch(rootPath, {
    ignored: /(^|[\/\\])\../,
    persistent: true
});

watcher
    .on('ready', () => console.log('ready'))
    .on('add', (path: string) => {
        console.log('add', path);

        songs.push(path.replace(rootPath, ''));
    })
    .on('change', path => console.log('change', path))
    .on('unlink', path => console.log('remove', path));

const httpServer = http.createServer((_, res) => {
    const nextSong = songs.pop();
    console.log(nextSong);

    res.writeHead(200, {"Content-Type": "text/plain"});
    res.end(nextSong);
});

httpServer.listen(8080);

process.on('SIGINT', () => process.exit());
process.on('SIGTERM', () => process.exit());
process.on('exit', () => {
    console.log('exit');
    return watcher.close();
});