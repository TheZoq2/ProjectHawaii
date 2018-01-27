class SimpleGame {

    constructor() {
        this.game = new Phaser.Game(1200, 600, Phaser.AUTO, "content", { preload: this.preload, create: this.create ,update:this.update});
    }

    game: Phaser.Game;
    timer;
    a:Number;

    preload() {
        this.game.load.image("logo", "phaser2.png");
    }

    create() {
        var logo = this.game.add.sprite(this.game.world.centerX, this.game.world.centerY, "logo");
        logo.anchor.setTo(0.5, 0.5);

        this.count = 0;

        this.text = this.game.add.text(this.game.world.centerX, this.game.world.centerY, "- You have clicked -\n0 times !", {
            font: "65px Arial",
            fill: "#ff0044",
            align: "center"
        });

        this.text.anchor.setTo(0.5, 0.5);

    }

    text;
    count;

    update() {

        this.game.input.onDown.addOnce(updateText, this);

    }


    
}

window.onload = () => {
    var game = new SimpleGame();
    this.timer = 1;
};

function updateText() {

    this.count++;

    this.text.setText(`- You have clicked -\n${this.count} times !`);

}