class Box {
  constructor(params) {
    this.canvas = params.canvas;
    this.id = params.id;
    this.x = params.x; this.y = params.y;
    this.z = params.z;
    this.width = params.width;
    this.height = params.height;
    this.type = params.type;
    this.content = params.content;
  }

  static getByNode(canvas, node) {
    this.canvas = canvas;
    var n = $(node);
    this.id = n.data('id');
    this.x = n.data('x');
    this.y = n.data('y');
    this.z = n.data('z');
    this.width = n.data('width');
    this.height = n.data('height');
    this.type = n.data('type');
  }

  place() {
    let div = $('<div></div>').data({
        id: this.id,
        x: this.x,
        y: this.y,
        z: this.z,
        width: this.width,
        height: this.height
      }).attr({
        'class': 'box js-box'
      }).css({
        left: this.x,
        top: this.y,
        zIndex: this.z,
        width: this.width,
        height: this.height
      });
    $(this.canvas).append(
      div.append($('<div class="box__handle-bar  js-box__handle-bar">'+this.id+'</div>'))
    );
    if (this.type === 'text') {
      let textarea = $('<textarea class="box__text"></textarea>').html(this.content);
      div.append(textarea);
    }
    div.draggable({handle: ".js-box__handle-bar" });
    div.resizable();
  }
}
