<template>
  <div id="app">
    <b-container class="justify-content-md-center">
      <b-tabs justified pills>
        <b-tab title="New person info" active :title-link-class="linkClass(0)">
          <PersonForm />
        </b-tab>
        <b-tab title="Search data" :title-link-class="linkClass(1)">
          <SearchForm />
        </b-tab>
      </b-tabs>
    </b-container>
  </div>
</template>

<script>
  import PersonForm from './components/PersonForm.vue';
  import SearchForm from './components/SearchForm.vue'

  export default {
    name: 'app',
    components: {
      PersonForm,
      SearchForm
    },
    methods: {
      linkClass(idx) {
        if (this.tabIndex === idx) {
          return ['bg-primary', 'text-light']
        } else {
          return ['bg-light', 'text-info']
        }
      }
    }
  }
</script>

<style>
  #app {
    font-family: 'Avenir', Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
    margin-top: 60px;
  }

  #app .nav-link.active {
    background-color: lightblue !important;
  }
</style>
