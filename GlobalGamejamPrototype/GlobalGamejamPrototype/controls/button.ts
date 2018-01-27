class Button {
    public name: string;
    //game: Phaser.Game;
    private buttonSprite: Phaser.Button;
    private graphics: Phaser.Graphics;

    public constructor(private game: Phaser.Game, public symbol: string, public x: number, public y: number) {
        this.name = "Button " + this.symbol + " sprite";

        this.preload();
        this.create();
    }

    private preload(): void {
        //this.game.load.
    }

    private create(): void {
        this.graphics = this.game.add.graphics(300, 300);
        this.graphics.lineStyle(0);
        this.graphics.beginFill(0xff0000);
        this.graphics.drawEllipse(this.x, this.y, 100, 50);
        this.graphics.endFill();

        //this.buttonSprite = this.game.add.button
        //    (this.x, this.y, this.name, this.onSymbolButtonClick);

        //this.buttonSprite.anchor.setTo(0.5, 0.5);

    }
    private update(): void {

    }

    private onSymbolButtonClick(): void {
        alert("Hello!");
    }

}
