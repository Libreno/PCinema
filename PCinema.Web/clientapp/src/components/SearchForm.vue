<template>
    <b-overlay :show="showSpinner" rounded="sm">
      <b-alert :show="showAlert" variant="danger">{{error}}</b-alert>
      <form class="pc-search-form" @submit.stop.prevent="onSubmit">
        <input type="text" v-model="searchString"/>
        <b-button type="submit" variant="primary" @onClick="onSubmit">Search</b-button>
      </form>
      <div id="search-results" class="mt-2 pc-search-results">
        <b-alert :show="nothingFound" variant="info">Matches not found.</b-alert>
        <b-pagination v-model="currentPage"
                      :total-rows="itemsTotalCount"
                      :per-page="itemsPerPage"
                      @change="pageChange"
                      v-if="itemsTotalCount > itemsPerPage"></b-pagination>
        <b-card v-bind:key="person.id" v-for="person in searchItemsOnPage">
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
              <span v-else>
                {{field.value}}
              </span>
            </div>
            <b>Matches:</b>
            <ol>
              <li v-bind:key="highlight.id"
                  v-for="highlight in person.highlights"
                  v-html="highlight.text">
              </li>
            </ol>
          </b-collapse>
        </b-card>
        <b-pagination v-model="currentPage"
                      :total-rows="itemsTotalCount"
                      :per-page="itemsPerPage"
                      @change="pageChange"
                      v-if="itemsTotalCount > itemsPerPage"></b-pagination>
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
        searchItemsOnPage: [],
        allSearchItems: [],
        itemsPerPage: 5,
        currentPage: 1,
        itemsTotalCount: null
      }
    },
    computed: {
      showAlert() {
        return Boolean(this.error !== undefined && this.error.length > 0);
      },
      nothingFound() {
        return this.itemsTotalCount === 0;
      }
    },
    methods: {
      onSubmit() {
        if (this.searchString === "") {
          return;
        }
        this.showSpinner = true;
        this.searchItemsOnPage = [];
        this.error = '';
        axios.get('/api/data/search', { params: { query: this.searchString }})
          .then(resp => {
            this.itemsTotalCount = resp.data.length;
            this.currentPage = 1;
            this.allSearchItems = resp.data.map((v, i) => {
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
            this.searchItemsOnPage = this.allSearchItems.slice((this.currentPage - 1) * this.itemsPerPage, this.currentPage * this.itemsPerPage);
          })
          .catch(err => {
            this.error = err.toString();
            console.error(err);
          })
          .finally(() => {
            this.showSpinner = false;
          })
      },
      pageChange(page) {
        this.searchItemsOnPage = this.allSearchItems.slice((page - 1) * this.itemsPerPage, page * this.itemsPerPage);
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
  #search-results .b-pagination {
    margin-top: 15px;
  }
</style>