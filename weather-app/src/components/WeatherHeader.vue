<template>
  <nav class="navbar navbar-default navbar-fixed-top">
    <div class="container">
      <div class="navbar-header">
        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
          <span class="icon-bar"></span>
          <span class="icon-bar"></span>
          <span class="icon-bar"></span>
        </button>
        <a class="navbar-brand">Weather Forecast</a>
      </div>
    </div>
  </nav>
</template>

<script>
export default {
  name: "WeatherHeader"
};
</script>

<style scoped>
.navbar {margin-bottom: 0;background-color: #f4511e;z-index: 9999;border: 0;font-size: 12px !important;line-height: 1.42857143 !important;letter-spacing: 4px;border-radius: 0;font-family: Montserrat, sans-serif;}.navbar .container .navbar-header{text-align: center;float: none;}
.navbar .container .navbar-header .navbar-brand {color: #fff !important;float: none;line-height: 50px; }
</style>
