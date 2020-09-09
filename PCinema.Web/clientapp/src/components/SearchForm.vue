<template>
    <b-overlay :show="showSpinner" rounded="sm">
      <b-alert :show="alertShow" variant="danger">{{error}}</b-alert>
      <form class="pc-search-form" @submit.stop.prevent="onSubmit">
        <input type="text" v-model="searchString"/>
        <b-button type="submit" variant="primary" @onClick="onSubmit">Search</b-button>
      </form>
      <div id="search-results" class="mt-2 pc-search-results">
        <div v-bind:key="person.id" v-for="person in searchResults">
          <div v-bind:key="field.id" v-for="field in person.fields.slice(0, 1)">
            <h4>{{field.value}}</h4>
          </div>
          <div v-bind:key="highlight.id"
               v-for="highlight in person.highlights.slice(0, 1)"
               v-html="highlight.text">
          </div>
          <b-link v-b-toggle="person.id">
            <span class="when-open">Hide full info &lt;&lt;</span><span class="when-closed">Show full info &gt;&gt;</span>
          </b-link>
          <b-collapse v-bind:id="person.id" class="mt-2">
            <div v-bind:key="field.id" v-for="field in person.fields">
              <b>{{field.name}}: </b>
              <div v-if="Array.isArray(field.value)">
                <ul>
                  <li v-bind:key="arrItem"
                      v-for="arrItem in field.value">
                    {{arrItem}}
                  </li>
                </ul>
              </div>
              <span v-if="!Array.isArray(field.value)">
                {{field.value}}
              </span>
            </div>
            <b>Matches:</b>
            <ul>
              <li v-bind:key="highlight.id"
                   v-for="highlight in person.highlights"
                   v-html="highlight.text">
              </li>
            </ul>
          </b-collapse>
        </div>
      </div>
    </b-overlay>
</template>

<script>
  import axios from 'axios';

  export default {
    data() {
      return {
        searchString: '',
        error: '',
        showSpinner: false,
        searchResults: []
      }
    },
    computed: {
      alertShow() {
        return Boolean(this.error !== undefined && this.error.length > 0);
      },
    },
    methods: {
      onSubmit() {
        if (this.searchString === "") {
          return;
        }
        this.showSpinner = true;
        this.searchResults = [];
        this.error = '';
        axios.get('/api/data/search', { params: { query: this.searchString }})
          .then(resp => {
            console.log(resp);
            this.searchResults = resp.data.map((v, i) => {
              let person = JSON.parse(v.fullText);
              return {
                id: "person" + i,
                fields: Object.entries(person).map((e, i) => {
                  return {
                    id: "field" + i,
                    name: e[0].charAt(0).toUpperCase() + e[0].slice(1),
                    value: e[1]
                  }
                }),
                highlights: v.highlights.map((m, i) => {
                  return {
                    id: "highlight" + i,
                    text: m
                  }
                })
              };
            });
            console.log(this.searchResults);
          })
          .catch(err => {
            this.error = err.toString();
            console.error(err);
          })
          .finally(() => {
            this.showSpinner = false;
          })
      }
    }
  }
</script>

<style>
  .pc-search-form {
    display:flex;
    padding-top: 15px;
  }
  .pc-search-form input{
    width: 100%;
  }
  .pc-search-results {
    text-align:left;
  }
  .pc-search-results em {
    background-color: yellow;
  }
  .collapsed > .when-open,
  .not-collapsed > .when-closed {
    display: none;
  }
  #search-results a {
    color: #17a2b8;
  }
  #search-results h4 {
    margin-top: 15px;
  }
</style>