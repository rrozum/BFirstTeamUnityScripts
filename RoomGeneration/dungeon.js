SEED = CryptoJS.MD5("" + new Date().getTime()).toString()
MAP_SIZE = 50
SQUARE   = 400 / MAP_SIZE
N_ITERATIONS = 4
W_RATIO = 0.45
H_RATIO = 0.45
DISCARD_BY_RATIO = true
D_GRID = true
D_BSP = true
D_ROOMS = true
D_PATHS = true

function random(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min)
}

function rectIntersect(r1, r2) {
    return !(
        r1.x > r2.x + r2.w ||
        r1.x + r1.w < r2.x ||
        r1.y > r2.y + r2.h ||
        r1.y + r1.h < r2.y
    )
}

var Point = function(x, y) {
    this.x = x
    this.y = y
}

var Room = function(x, y, w, h) {
    this.x = x
    this.y = y
    this.w = w
    this.h = h
    this.center = new Point(this.x + this.w/2, this.y + this.h/2)
}

Room.prototype.paint = function(c) {
    c.fillStyle = "#888"
    c.fillRect(this.x * SQUARE, this.y * SQUARE,
               this.w * SQUARE, this.h * SQUARE)
}

Room.prototype.drawPath = function(c, point) {
    c.beginPath()
    c.lineWidth   = SQUARE
    c.strokeStyle = "#888"
    c.moveTo(this.center.x * SQUARE, this.center.y * SQUARE)
    c.lineTo(point.x * SQUARE, point.y * SQUARE)
    c.stroke()
}

var RoomContainer = function(x, y, w, h) {
    Room.call(this, x, y, w, h)
    this.room = undefined
}

RoomContainer.prototype = Object.create(Room.prototype)
RoomContainer.prototype.constructor = RoomContainer
RoomContainer.prototype.paint = function(c) {
    c.strokeStyle = "#0F0"
    c.lineWidth   = 2
    c.strokeRect(this.x * SQUARE, this.y * SQUARE,
               this.w * SQUARE, this.h * SQUARE)
}

RoomContainer.prototype.growRoom = function() {
    var x, y, w, h
    x = this.x + random(0, Math.floor(this.w/3))
    y = this.y + random(0, Math.floor(this.h/3))
    w = this.w - (x - this.x)
    h = this.h - (y - this.y)
    w -= random(0, w/3)
    h -= random(0, h/3)
    this.room = new Room(x, y, w, h)
}

function random_split(room) {
    var r1, r2
    if (random(0, 1) == 0) {
        // Vertical
        r1 = new RoomContainer(
            room.x, room.y,             // r1.x, r1.y
            random(1, room.w), room.h   // r1.w, r1.h
        )
        r2 = new RoomContainer(
            room.x + r1.w, room.y,      // r2.x, r2.y
            room.w - r1.w, room.h       // r2.w, r2.h
        )
        if (DISCARD_BY_RATIO) {
            var r1_w_ratio = r1.w / r1.h
            var r2_w_ratio = r2.w / r2.h
            if (r1_w_ratio < W_RATIO || r2_w_ratio < W_RATIO) {
                return random_split(room)
            }
        }
    } else {
        // Horizontal
        r1 = new RoomContainer(
            room.x, room.y,             // r1.x, r1.y
            room.w, random(1, room.h)   // r1.w, r1.h
        )
        r2 = new RoomContainer(
            room.x, room.y + r1.h,      // r2.x, r2.y
            room.w, room.h - r1.h       // r2.w, r2.h
        )
        if (DISCARD_BY_RATIO) {
            var r1_h_ratio = r1.h / r1.w
            var r2_h_ratio = r2.h / r2.w
            if (r1_h_ratio < H_RATIO || r2_h_ratio < H_RATIO) {
                return random_split(room)
            }
        }
    }
    return [r1, r2]
}

function split_room(room, iter) {
    var Root = new Tree(room)
    room.paint(document.getElementById('viewport').getContext('2d'))
    if (iter != 0) {
        var sr = random_split(room)
        Root.lchild = split_room(sr[0], iter-1)
        Root.rchild = split_room(sr[1], iter-1)
    }
    return Root
}

var Tree = function( leaf ) {
    this.leaf = leaf
    this.lchild = undefined
    this.rchild = undefined
}

Tree.prototype.print = function() {
    console.log(this.leaf)
    if (this.lchild !== undefined)
        this.lchild.print()
    if (this.rchild !== undefined)
        this.rchild.print()
}

Tree.prototype.getLeafs = function() {
    if (this.lchild === undefined && this.rchild === undefined)
        return [this.leaf]
    else
        return [].concat(this.lchild.getLeafs(), this.rchild.getLeafs())
}

Tree.prototype.getLevel = function(level, queue) {
    if (queue === undefined)
        queue = []
    if (level == 1) {
        queue.push(this)
    } else {
        if (this.lchild !== undefined)
            this.lchild.getLevel(level-1, queue)
        if (this.rchild !== undefined)
            this.rchild.getLevel(level-1, queue)
    }
    return queue
}

Tree.prototype.paint = function(c) {
    var c = c
    this.leaf.paint(c)
    if (this.lchild !== undefined)
        this.lchild.paint(c)
    if (this.rchild !== undefined)
        this.rchild.paint(c)
}

Map = function(width, height, c) {
    this.c = c
    this.width = width
    this.height = height
    this.rooms = []
    this.init()
}

Map.prototype.init = function() {
    var main_room = new RoomContainer(0, 0, MAP_SIZE, MAP_SIZE)
    this.room_tree = split_room(main_room, N_ITERATIONS)
    this.growRooms()
}

Map.prototype.growRooms = function() {
    var leafs = this.room_tree.getLeafs()
    for (var i = 0; i < leafs.length; i++) {
        leafs[i].growRoom()
        this.rooms.push(leafs[i].room)
    }
}

Map.prototype.clear = function() {
    this.c.fillStyle = "#000"
    this.c.fillRect(0, 0, this.width, this.height)
}

Map.prototype.drawPaths = function(tree) {
    if (tree.lchild !== undefined && tree.rchild !== undefined) {
        tree.lchild.leaf.drawPath(this.c, tree.rchild.leaf.center)
        this.drawPaths(tree.lchild)
        this.drawPaths(tree.rchild)
    }
}


Map.prototype.drawGrid = function() {
    var c = this.c
    c.beginPath()
    c.strokeStyle = "rgba(255,255,255,0.4)"
    c.lineWidth = 0.5
    for (var i = 0; i < MAP_SIZE; i++) {
        c.moveTo(i * SQUARE, 0)
        c.lineTo(i * SQUARE, MAP_SIZE * SQUARE)
        c.moveTo(0, i * SQUARE)
        c.lineTo(MAP_SIZE * SQUARE, i * SQUARE)
    }
    c.stroke()
    c.closePath()
}

Map.prototype.drawContainers = function() {
    this.room_tree.paint(this.c)
}

Map.prototype.drawRooms = function() {
    for (var i = 0; i < this.rooms.length; i++)
        this.rooms[i].paint(this.c)
}

Map.prototype.paint = function() {
    this.clear()
    if (D_BSP)
        this.drawContainers()
    if (D_ROOMS)
        this.drawRooms()
    if (D_PATHS)
        this.drawPaths(this.room_tree)
    if (D_GRID)
        this.drawGrid()
}

