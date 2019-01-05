<template>
  <div>
    <div class="col-md-3" v-for="uniqueDate in WeatherWidgetDates">
      <div class="panel panel-default text-center">
        <div class="panel-heading">
          <span>{{getDateTitle(uniqueDate)}}</span>
        </div>
        <div class="title">
          <div class="time">
            <span title="Temprature">
              <i class="fas fa-clock"></i>
              <span class="icon">Time</span>
            </span>
          </div>
          <div class="temp">
            <span title="Temprature">
              <i class="fas fa-temperature-low"></i>
              <span class="icon">(°C)</span>
            </span>
          </div>
          <div class="wind">
            <span title="Wind">
              <i class="fas fa-wind"></i>
              <span class="icon">(km/h)</span>
            </span>
          </div>
          <div class="humidity">
            <span title="Humidity">
              <i class="fas fa-tint"></i>
              <span class="icon">(%)</span>
            </span>
          </div>
        </div>
        <div
          class="panel-body"
          v-for="weatherInformation in WeatherWidgetData"
          v-if="isDataInDate(uniqueDate, weatherInformation) == true "
        >
          <div class="data">
            <div class="time">{{getHoursFromDate(weatherInformation.epochTime)}}</div>
            <div class="temp">{{weatherInformation.temprature}}</div>
            <div class="wind">{{weatherInformation.wind}}</div>
            <div class="humidity">{{weatherInformation.humidity}}</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "WeatherWidget",
  props: ["WeatherWidgetData", "WeatherWidgetDates", "Weekday"],
  methods: {
    isDataInDate(date, data) {
      return this.isSameDay(date, new Date(data.epochTime * 1000));
    },
    getHoursFromDate(dateInEpoch) {
      var date = new Date(dateInEpoch * 1000);
      //Formating time in two digit
      return (
        ("0" + date.getHours()).substr(-2) +
        ":" +
        ("0" + date.getMinutes()).substr(-2)
      );
    },
    getDateTitle(date) {
      var today = new Date();
      var tomorrow = new Date();
      tomorrow.setDate(today.getDate() + 1);
      if (this.isSameDay(date, today)) {
        return "Today";
      } else if (this.isSameDay(date, tomorrow)) {
        return "Tomorrow";
      } else {
        return this.Weekday[date.getDay()];
      }
    },
    isSameDay(date1, date2) {
      //Removing time, just comparing date
      if (date1.setHours(0, 0, 0, 0) === date2.setHours(0, 0, 0, 0)) {
        return true;
      }
      return false;
    }
  }
};
</script>
<style lang="scss" scoped>
$theme-back-color: #f4511e;
$white-color: #f5efef;
$grey-color: #403f3f;
.tableTheme {
  .time {
      float: left;
      width: 35%;
    }
    .temp {
      float: left;
      width: 21.66%;
      background: #edbf09c7;
      color: $grey-color;
    }
    .wind {
      float: left;
      width: 21.66%;
      background: #2cadda;
      color: $grey-color;
    }
    .humidity {
      float: left;
      width: 21.66%;
      background: #1eab83;
      color: $grey-color;
    }
};

.panel {
  border: 1px solid $theme-back-color;
  border-radius: 0 !important;
  transition: box-shadow 0.5s;
  width: 100%;

  &:hover {
    box-shadow: 5px 0px 40px $grey-color;
  }

  .panel-heading {
    padding: 0;
    color: $white-color !important;
    background-color: $theme-back-color !important;
    border-bottom: 1px solid transparent;
    border-top-left-radius: 0px;
    border-top-right-radius: 0px;
    border-bottom-left-radius: 0px;
    border-bottom-right-radius: 0px;

    span {
      color: $white-color;
      font-size: 24px;
    }
  }

  .title {
    @extend .tableTheme;
    background: $theme-back-color;
    margin: 0;
    border-top: 1px #b3360f solid;
    display: flex;
    span {
      color: $white-color;
    }
    div {
      span {
        color: $grey-color;
        span.icon {
          margin-left: 4px;
        }
      }
    }
  }

  .panel-body {
    @extend .tableTheme;
    border-top: 1px solid $theme-back-color;
    padding: 0;
  }
}
</style>
