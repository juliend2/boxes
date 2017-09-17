const App = {
  init: (canvas) => {
    [{
      canvas: canvas,
      id: 1,
      x: 100,
      y: 100,
      z: 0,
      width: 400,
      height: 400,
      type: 'text',
      content: 'Interdum et malesuada fames ac ante ipsum primis in faucibus. Proin id velit molestie, vehicula mauris at, ultrices mauris. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam erat volutpat. Nam porttitor lorem in condimentum tempor. Aenean nec leo ante. Etiam egestas, risus vel sagittis venenatis, tortor orci tempor lectus, non ullamcorper metus risus sed metus. Ut quis dolor ac lectus condimentum condimentum in id leo. Proin hendrerit ex eget enim varius tincidunt. Nulla eu dolor ac dui dapibus feugiat et eu orci. Curabitur porta mauris nec finibus pellentesque. Aliquam facilisis eu nunc sed scelerisque. Ut et lacus id eros iaculis efficitur non maximus ante. Proin in sapien tempor magna pulvinar aliquet sed eu neque. Morbi non volutpat ex, at lobortis tortor. Phasellus eget tristique augue.'
    }].forEach((params)=>{
      let box = new Box(params);
      box.place();
    });
  }
};
