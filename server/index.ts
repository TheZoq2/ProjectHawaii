import * as socketio from "socket.io" 
import * as http from "http" 
const express = require('express')

function user_handler(socket: SocketIO.Socket): void {
    console.log("user connected")
    socket.on('disconnect', () => {
        console.log("client disconnected")
    })
}

function main(): void {
    let app = express()
    let http_server = new http.Server(app)
    let io = socketio(http_server)

    // Send index.html for / requests
    app.get('/', (req: any, res: any) => {
        //res.send("<h1>hello world</h1>")
        res.sendFile(__dirname + '/index.html')
    })

    io.on('connection', user_handler)

    // Start the http server
    http_server.listen(8080, function() {
        console.log('listening on http://localhost:8080')
    })
}


main()
