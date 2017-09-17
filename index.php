<?php

?>
<!DOCTYPE html>
<!--[if lt IE 9 ]> <html lang='en' class='lt-ie9'> <![endif]-->
<!--[if IE 9 ]>    <html lang='en' class='ie9'> <![endif]-->
<!--[if (gt IE 9)|!(IE)]><!--> <html lang='en'> <!--<![endif]-->
<head>
  <meta charset="utf-8"/>
  <title></title>

  <link rel="stylesheet" href="/assets/js/lib/jquery-ui-1.12.1/jquery-ui.min.css"/>
  <link rel="stylesheet" href="/assets/css/styles.css"/>
  <meta name="viewport" content="width=device-width, initial-scale=1">
</head>
<body>
  <div class="canvas" id="app">
  </div>

  <script src="/assets/js/lib/jquery-3.2.1.min.js"></script>
  <script src="/assets/js/lib/jquery-ui-1.12.1/jquery-ui.min.js"></script>
  <script src="/assets/js/box.js"></script>
  <script src="/assets/js/app.js"></script>
  <script type="text/javascript">
  $(function(){
    App.init($('#app')[0]);
  });
  </script>
</body>
</html>
