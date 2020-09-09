<template>
  <b-overlay :show="showSpinner" rounded="sm">
    <b-alert :show="alertShow" variant="danger">{{error}}</b-alert>
    <b-form @submit.stop.prevent="onSubmit" v-if="showForm">
      <b-form-group id="form-name-group"
                    label-cols-lg="2"
                    label="Full name:"
                    label-for="input-name">
        <b-form-input id="input-name"
                      v-model="$v.form.name.$model"
                      :state="validateState('name')"
                      aria-describedby="input-name-feedback">
        </b-form-input>
        <b-form-invalid-feedback id="input-name-feedback">This is a required field.</b-form-invalid-feedback>
      </b-form-group>
      <b-form-group id="form-birthdate-group"
                    label-cols-lg="2"
                    label="Birthdate:"
                    label-for="input-birthdate"
                    :state="validateState('birthDate')">
        <b-form-datepicker id="input-birthDate"
                            show-decade-nav
                            menu-class="w-100"
                            calendar-width="100%"
                            hide-header
                            locale="en-US"
                            min="1800-01-01"
                            :max="maxDate"
                            v-model="$v.form.birthDate.$model"
                            :state="validateState('birthDate')"
                            aria-describedby="input-birthdate-live-feedback"
                            />
        <b-form-invalid-feedback id="input-birthDate-feedback">This is a required field.</b-form-invalid-feedback>
      </b-form-group>
      <b-form-group id="form-sex-group"
                    label-cols-lg="2"
                    label="Sex:"
                    label-for="input-sex-group">
        <b-form-radio-group id="input-sex-group"
                            class="row-list-values"
                            v-model="$v.form.sex.$model"
                            :state="validateState('sex')"
                            aria-describedby="input-sex-feedback">
          <b-form-radio value="Male">Male</b-form-radio>
          <b-form-radio value="Female">Female</b-form-radio>
        </b-form-radio-group>
        <div id="input-sex-feedback" v-if="$v.form.sex.$error" class="pc-error-message">This is a required field.</div>
      </b-form-group>
      <b-form-group id="from-bio-group"
                    label-cols-lg="2"
                    label="Short bio:"
                    label-for="input-bio">
        <b-form-textarea id="input-bio"
                          rows="3"
                          max-rows="6"
                          v-model="$v.form.bio.$model"
                          :state="validateState('bio')"
                          aria-describedby="input-bio-feedback">
        </b-form-textarea>
        <b-form-invalid-feedback id="input-bio-feedback">This is a required field.</b-form-invalid-feedback>
      </b-form-group>
      <b-form-group id="form-children-group"
                    label-cols-lg="2"
                    label="Children:"
                    label-for="input-children">
        <b-form-select id="input-children"
                        v-model="$v.form.children.$model"
                        :state="validateState('children')"
                        aria-describedby="input-children-feedback">
          <b-form-select-option value="0">0</b-form-select-option>
          <b-form-select-option value="1">1</b-form-select-option>
          <b-form-select-option value="2">2</b-form-select-option>
          <b-form-select-option value="3">3</b-form-select-option>
          <b-form-select-option value="4">4</b-form-select-option>
          <b-form-select-option value="5">5</b-form-select-option>
          <b-form-select-option value="6">6</b-form-select-option>
          <b-form-select-option value="7">7</b-form-select-option>
          <b-form-select-option value="8">8</b-form-select-option>
        </b-form-select>
        <b-form-invalid-feedback id="input-children-feedback">This is a required field.</b-form-invalid-feedback>
      </b-form-group>
      <b-form-group id="form-professions-group"
                    label-cols-lg="2"
                    label="Professions:"
                    label-for="input-professions">
        <b-form-checkbox-group id="input-professions-checkbox-group"
                                v-model="form.professions"
                                :state="validateState('professions')"
                                class="row-list-values"
                                aria-describedby="input-professions-feedback">
          <b-form-checkbox value="actor" inline>Actor(Actress)</b-form-checkbox>
          <b-form-checkbox value="cinematographer" inline>Cinematographer</b-form-checkbox>
          <b-form-checkbox value="composer" inline>Composer</b-form-checkbox>
          <b-form-checkbox value="film_editor" inline>Film editor</b-form-checkbox>
        </b-form-checkbox-group>
        <div id="input-professions-feedback" 
             v-if="$v.form.professions.$error" 
             class="pc-error-message">This is a required field.</div>
      </b-form-group>
      <b-button type="submit" variant="primary" @onClick="onSubmit">Submit</b-button>
    </b-form>
    <div v-if="!showForm">
      <b-alert variant="success" show>Record added successfully.</b-alert>
      <b-button type="submit" @click="addMoreClick" variant="primary" class="pc-add-more-button">Add more</b-button>
    </div>
  </b-overlay>
</template>

<script>
  import axios from 'axios';
  import { validationMixin } from "vuelidate";
  import { required } from "vuelidate/lib/validators";

  export default {
    mixins: [validationMixin],
    name: 'PersonForm',
    data() {
      return {
        form: {
          name: null,
          sex: null,
          bio: null,
          children: null,
          birthDate: null,
          professions: null,
        },
        maxDate: new Date(),
        error: '',
        showSpinner: false,
        showForm: true
      }
    },
    validations: {
      form: {
        name: {
          required
        },
        sex: {
          required
        },
        bio: {
          required
        },
        children: {
          required
        },
        birthDate: {
          required
        },
        professions: {
          required
        }
      }
    },
    computed: {
      alertShow() {
        return Boolean(this.error !== undefined && this.error.length > 0);
      }
    },
    methods: {
      validateState(name) {
        const { $dirty, $error } = this.$v.form[name];
        return $dirty ? !$error : null;
      },
      resetForm() {
        this.form = {
          name: null,
          sex: null,
          bio: null,
          children: null,
          birthDate: null,
          professions: null,
        };

        this.$nextTick(() => {
          this.$v.$reset();
        });
      },
      onSubmit() {
        this.$v.form.$touch();
        if (this.$v.form.$anyError) {
          return;
        }
        this.showSpinner = true;
        console.log(JSON.stringify(this.form));
        axios.post('/api/data/addentry', this.form)
          .then(resp => {
            console.log(resp);
            this.showForm = false;
            this.resetForm();
          })
          .catch(err => {
            this.error = err.toString();
            console.error(err);
          })
          .finally(() => {
            this.showSpinner = false;
          })
      },
      addMoreClick() {
        this.showForm = true;
      }
    }
  }
</script>

<style>
  .row-list-values {
    text-align: left;
  }
  .row-list-values-center {
    display: flex;
    align-items: center;
  }
  .pc-error-message {
    font-size: 80%;
    color: #dc3545;
  }
  .alert {
    margin: 15px 0 0 0;
  }
  label {
    text-align: left;
  }
  .pc-add-more-button {
    margin-top: 15px;
  }
  @media (min-width:1024px) {
    #form-sex-group label {
      padding-top: 0px;
    }
    #form-professions-group label {
      padding-top: 0px;
    }
    #form-name-group {
      padding-top: 15px;
    }
  }
  @media (max-width:1023px) {
    #input-professions-checkbox-group {
      display: flex;
      flex-direction:column;
    }
  }
</style>