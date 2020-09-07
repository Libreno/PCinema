<template>
    <b-overlay :show="showSpinner" rounded="sm">
      <b-alert :show="alertShow" variant="danger">{{error}}</b-alert>
      <form class="pc-search-form" @submit.stop.prevent="onSubmit">
        <input type="text" v-model="searchString"/>
        <b-button type="submit" variant="primary" @onClick="onSubmit">Search</b-button>
      </form>
      <div id="search-results" class="mt-2">
        <div v-bind:key="person.id" v-for="person in searchResults">
          <div v-bind:key="field.id" v-for="field in person.fields">
            {{field.name}}: {{field.value}}
          </div>
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
        axios.get('/api/data/search', { params: { query: this.searchString }})
          .then(resp => {
            this.searchResults = resp.data.map((v, i) => {
              let person = JSON.parse(v.fullText);
              return {
                id: i,
                fields: Object.entries(person).map((e, i) => {
                  return {
                    id: i,
                    name: e[0],
                    value: e[1]
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
</style>