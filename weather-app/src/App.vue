<template>
  <div id="app">
    <WeatherHeader></WeatherHeader>
    <div class="container-fluid">
      <div class="row">
        <div class="col-md-3">
          <div class="text-left" v-if="this.Response !== null">
            <h2>{{City}} - {{Country}}</h2>
            <h4>{{getTodayString()}}</h4>
          </div>
        </div>
        <div class="col-md-3" v-if="this.Response !== null">
          <div class="btnOuterWrapper">
            <div class="btnWrapper">
              <button class="btn chart" @click="showChart()">
                <i class="fas fa-chart-line"></i>
                Graphs
              </button>
              <div class="arrow-down" v-if="this.ShowChart===true"></div>
            </div>
            <div class="btnWrapper">
              <button class="btn table" @click="showTables()">
                <i class="fas fa-table"></i>
                Tabular Format
              </button>
              <div class="arrow-down" v-if="this.ShowChart===false"></div>
            </div>
          </div>
        </div>
        <div class="col-md-3" v-if="this.Response !== null">
          <div class="btnOuterWrapper" v-if="this.ShowChart===true">
            <div class="btnWrapper">
              <button class="btn temprature" @click="showTempratureChart()">
                <i class="fas fa-temperature-low"></i>
                Temprature
              </button>
              <div class="arrow-down" v-if="this.VisibleChart===1"></div>
            </div>
            <div class="btnWrapper">
              <button class="btn wind" @click="showWindChart()">
                <i class="fas fa-wind"></i>
                Wind
              </button>
              <div class="arrow-down" v-if="this.VisibleChart===2"></div>
            </div>
            <div class="btnWrapper">
              <button class="btn humidity" @click="showHumidityChart()">
                <i class="fas fa-tint"></i>
                Humidity
              </button>
              <div class="arrow-down" v-if="this.VisibleChart===3"></div>
            </div>
          </div>
        </div>
        <div class="col-md-3">
          <div class="search-container">
            <WeatherSearchCity
              v-if="this.Response!==null"
              @update="onCityChange"
              :defaultValue="this.SelectedCity"
              :defaultDisplay="this.City +' - '+ this.Country"
              :historyData="this.HistoryCities"
              source="http://localhost:5002/api/city/"
            ></WeatherSearchCity>
          </div>
        </div>
      </div>
      <div class="row top-buffer">
        <div v-if="this.Response !== null && this.ShowChart===true">
          <WeatherGraph
            :weatherChartData="this.ChartData"
            :weatherChartOptions="this.DefaultChartOptions"
          ></WeatherGraph>
        </div>
        <div class="col-md-12" v-if="this.Response !== null && this.ShowChart===false">
          <WeatherWidget
            v-bind:WeatherWidgetDates="this.UniqueDates"
            v-bind:WeatherWidgetData="this.Response.weatherInformation"
            v-bind:Weekday="this.Weekday"
          ></WeatherWidget>
        </div>
      </div>
      <WeatherFooter v-if="this.Response !== null"></WeatherFooter>
      <div class="alert alert-danger" v-if="this.AppError !== null">
        <strong>Error!</strong>
        {{AppError}}
      </div>
    </div>
  </div>
</template>

<script>
import WeatherHeader from "./components/WeatherHeader";
import WeatherFooter from "./components/WeatherFooter";
import WeatherSearchCity from "./components/WeatherSearchCity";
import WeatherGraph from "./components/WeatherGraph";
import WeatherWidget from "./components/WeatherWidget";
import axios from "axios";

export default {
  name: "App",
  components: {
    WeatherHeader,
    WeatherFooter,
    WeatherSearchCity,
    WeatherGraph,
    WeatherWidget
  },
  methods: {
    loadDefaultCity() {
      this.SelectedCity = 2879139;
      this.getWeatherData(this.SelectedCity);
    },
    onCityChange(newCity) {
      this.SelectedCity = newCity.value;
      this.getWeatherData(this.SelectedCity);
    },
    getUserHistoryCities() {
      var historyCitiesStr = localStorage.cachedHistory;
      if (historyCitiesStr) {
        var cachedHistory = JSON.parse(historyCitiesStr);
        this.HistoryCities = Object.keys(cachedHistory).map(function(key) {
          return cachedHistory[key];
        });
      }
    },
    getWeatherData(cityId) {
      var cachedData = this.getDataFromCache(cityId);
      if (cachedData) {
        this.IsFromCache = true;
        this.updateUI(cachedData);
      } else {
        axios
          .get("http://localhost:5002/api/weather/" + cityId)
          .then(response => {
            this.Status = response.data.status;
            this.Message = response.data.message;
            if (response.data.status == "200") {
              this.IsFromCache = false;
              this.updateUI(response.data);
            } else {
              this.AppError = response.data.message;
            }
          })
          .catch(error => {
            this.AppError = error.message;
            if (
              error.response &&
              error.response.data &&
              error.response.data.message
            ) {
              this.AppError = error.response.data.message;
            }
          });
      }
    },
    updateUI(response) {
      this.Response = response;
      this.DateTitlesForChart = response.weatherInformation.map(a =>
        this.getDateTitleAndHoursFromDate(a.epochTime)
      );
      this.City = response.city;
      this.Country = response.country;
      var uniqueDates = [];
      for (var i = 0; i < response.weatherInformation.length; i++) {
        var createdDate = new Date(
          response.weatherInformation[i].epochTime * 1000
        );
        if (!this.isDateInArray(createdDate, uniqueDates)) {
          uniqueDates.push(createdDate);
        }
      }
      this.UniqueDates = uniqueDates;
      this.showTempratureChart();
      this.AppError = null;
    },
    getDataFromCache(cityId) {
      //Cached data for today only
      if (localStorage.cachedDataFor != new Date().getDate()) {
        localStorage.removeItem("cachedData");
      }
      var cachedData = localStorage.cachedData;
      if (cachedData) {
        var responses = JSON.parse(cachedData);
        return responses[cityId];
      }
    },
    setDataInCache(newResponse) {
      //Cache data for the city
      localStorage.cachedDataFor = new Date().getDate();
      var cachedData = localStorage.cachedData;
      // create an empty array
      var responses = {};
      // create an empty array
      //If data already exist take it
      if (cachedData) {
        responses = JSON.parse(cachedData);
      }
      responses[this.SelectedCity] = newResponse;
      localStorage.cachedData = JSON.stringify(responses);

      //Cache data for the history
      var cachedHistory = localStorage.cachedHistory;
      var history = {};
      if (cachedHistory) {
        history = JSON.parse(cachedHistory);
      }
      history[this.SelectedCity] = {
        id: this.SelectedCity,
        name: this.City,
        country: this.Country
      };
      localStorage.cachedHistory = JSON.stringify(history);
      this.getUserHistoryCities();
    },
    isDateInArray(needle, haystack) {
      for (var i = 0; i < haystack.length; i++) {
        if (needle.setHours(0, 0, 0, 0) === haystack[i].setHours(0, 0, 0, 0)) {
          return true;
        }
      }
      return false;
    },
    getDateTitleAndHoursFromDate(dateInEpoch) {
      var title = "";
      var currentDate = new Date(dateInEpoch * 1000);
      var date = new Date(dateInEpoch * 1000);
      var today = new Date();
      var tomorrow = new Date();
      tomorrow.setDate(today.getDate() + 1);
      if (this.isSameDay(date, today)) {
        title = "Today";
      } else if (this.isSameDay(date, tomorrow)) {
        title = "Tomorrow";
      } else {
        title = this.Weekday[date.getDay()];
      }

      //Formating date title for chart
      return (
        title +
        " " +
        ("0" + currentDate.getHours()).substr(-2) +
        ":" +
        ("0" + currentDate.getMinutes()).substr(-2)
      );
    },
    getTodayString() {
      var date = new Date();
      var options = {
        weekday: "long",
        year: "numeric",
        month: "long",
        day: "numeric"
      };
      //Formating the date in local format
      return date.toLocaleDateString("en-US", options);
    },
    isSameDay(date1, date2) {
      //Removing time, just comparing date
      if (date1.setHours(0, 0, 0, 0) === date2.setHours(0, 0, 0, 0)) {
        return true;
      }
      return false;
    },
    showTempratureChart() {
      var tempratureData = this.Response.weatherInformation.map(a =>
        a.temprature.toString()
      );
      this.VisibleChart = 1;
      this.ChartData = {
        labels: this.DateTitlesForChart,
        datasets: [
          {
            label: "Temprature",
            backgroundColor: "#edbf09c7",
            data: tempratureData
          }
        ]
      };
    },
    showWindChart() {
      var windData = this.Response.weatherInformation.map(a =>
        a.wind.toString()
      );
      this.VisibleChart = 2;
      this.ChartData = {
        labels: this.DateTitlesForChart,
        datasets: [
          {
            label: "Wind",
            backgroundColor: "#2cadda",
            data: windData
          }
        ]
      };
    },
    showHumidityChart() {
      var humidityData = this.Response.weatherInformation.map(a =>
        a.humidity.toString()
      );
      this.VisibleChart = 3;
      this.ChartData = {
        labels: this.DateTitlesForChart,
        datasets: [
          {
            label: "Humidity",
            backgroundColor: "#1eab83",
            data: humidityData
          }
        ]
      };
    },
    showChart() {
      this.ShowChart = true;
    },
    showTables() {
      this.ShowChart = false;
    }
  },
  data: function() {
    return {
      Status: null,
      Message: null,
      SelectedCity: null,
      City: null,
      Country: null,
      Response: null,
      Days: null,
      UniqueDates: [],
      Weekday: [
        "Sunday",
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday"
      ],
      ChartData: null,
      VisibleChart: 1,
      ShowChart: true,
      DateTitlesForChart: [],
      IsFromCache: false,
      HistoryCities: false,
      AppError: null,
      DefaultChartOptions: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          yAxes: [
            {
              gridLines: {
                display: false
              }
            }
          ],
          xAxes: [
            {
              gridLines: {
                display: false
              }
            }
          ]
        }
      }
    };
  },
  watch: {
    Response(newResponse) {
      if (!this.IsFromCache) this.setDataInCache(newResponse);
    }
  },
  mounted: function() {
    this.loadDefaultCity();
    this.getUserHistoryCities();
  }
};
</script>

<style lang="scss" scoped>
$grey-color: #403f3f;
$white-color: #f5efef;

h2 {
  font-size: 24px;
  text-transform: uppercase;
  color: $grey-color;
  font-weight: 600;
  margin: 0;
}
h4 {
  font-size: 19px;
  line-height: 1.375em;
  color: $grey-color;
  font-weight: 400;
}
.container-fluid {
  padding: 60px 50px;
}
.top-buffer {
  margin-top: 20px;
}

.btnOuterWrapper {
  display: inline-block;
  .btnWrapper {
    display: inline-block;
    margin-right: 5px;
    .btn.temprature {
      color: $grey-color;
      background: #edbf09c7;
    }

    .btn.wind {
      color: $grey-color;
      background: #2cadda;
    }
    .btn.humidity {
      color: $grey-color;
      background: #1eab83;
    }
    .btn.chart {
      color: $grey-color;
      background: #2fbe17;
    }
    .btn.table {
      color: $grey-color;
      background: #f4511e;
    }
    .arrow-down {
      width: 0;
      height: 0;
      border-left: 5px solid transparent;
      border-right: 5px solid transparent;
      border-top: 5px solid black;
      margin: 0 auto;
    }
  }
}
</style>