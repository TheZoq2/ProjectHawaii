var _this = this;
var SimpleGame = (function () {
    function SimpleGame() {
        this.game = new Phaser.Game(1200, 600, Phaser.AUTO, "content", { preload: this.preload, create: this.create, update: this.update });
    }
    SimpleGame.prototype.preload = function () {
        this.game.load.image("logo", "phaser2.png");
    };
    SimpleGame.prototype.create = function () {
        var logo = this.game.add.sprite(this.game.world.centerX, this.game.world.centerY, "logo");
        logo.anchor.setTo(0.5, 0.5);
        this.count = 0;
        this.text = this.game.add.text(this.game.world.centerX, this.game.world.centerY, "- You have clicked -\n0 times !", {
            font: "65px Arial",
            fill: "#ff0044",
            align: "center"
        });
        this.text.anchor.setTo(0.5, 0.5);
    };
    SimpleGame.prototype.update = function () {
        this.game.input.onDown.addOnce(updateText, this);
    };
    return SimpleGame;
}());
window.onload = function () {
    var game = new SimpleGame();
    _this.timer = 1;
};
function updateText() {
    this.count++;
    this.text.setText("- You have clicked -\n" + this.count + " times !");
}
//# sourceMappingURL=app.js.map