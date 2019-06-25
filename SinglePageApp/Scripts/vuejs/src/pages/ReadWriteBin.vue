<template>
  <v-container>
    <TitleLink title="Read Write Binary File"></TitleLink>
    <v-layout row wrap>
      <v-flex xs12 sm8 md6 py-2>
        <h2 class="red--text text-xs-center">Write Binary File</h2>
        <v-textarea outline name="input-7-4" label="Write to Bin file" v-model="content"></v-textarea>
        <v-btn raised color="info" @click="writeFile">Write File</v-btn>
      </v-flex>
    </v-layout>

    <v-layout row wrap>
      <v-flex xs12 sm8 md6 py-2>
        <v-divider></v-divider>
        <h2 class="red--text text-xs-center">Read Binary File</h2>
        <!-- Input File type -->
        <FileInput placeholder="Choose Bin File"/>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script>
import TitleLink from "../components/TitleLink";
import FileInput from '../components/FileInput'
export default {
  components:{
    FileInput, TitleLink
  },

  data() {
    return {
      content: "",
    };
  },

  methods: {
    writeFile() {
      this.$Progress.start();
      axios
        .post("/api/binfile?msg=" + this.content)
        .then(res => {
          toast.fire({
            type: "success",
            title: "Written Employee successfully"
          });

          this.$Progress.finish();
        })
        .catch(() => {
          Swal("Failed!", "Some error occurred", "warning");
          this.$Progress.fail();
        });
    },
  }
};
</script>