//import * as socketio from "socket.io-client"
const socket = require('socket.io-client')("http://localhost:8080")

socket.on('connect', () => {
    console.log('connected')
})
socket.on('event', (data:any) => {})
socket.on('disconnect', () => {})
socket.close()
