import fs = require('fs')
import app = require('express')
import http = require('http')


function test(message: [string, string]) {
    let result = message + " " + 5
    console.log(result)
}

test(["yolloswag", "your mom"])
