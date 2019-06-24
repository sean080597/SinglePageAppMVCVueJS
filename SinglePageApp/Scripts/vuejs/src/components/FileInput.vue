<template>
  <v-container grid-list-xs>
    <v-form @submit.prevent="readFile">
      <v-layout row wrap>
        <v-flex xs10>
          <v-text-field
            :placeholder="placeholder"
            prepend-icon="attach_file"
            single-line
            required
            disabled
            v-model="fileName"
            ref="fileTextField"
          ></v-text-field>
          <input type="file" accept=".bin" ref="fileInput" @change="onFilePicked" hidden>
        </v-flex>
        <v-btn small raised fab color="info" @click="pickFile">
          <v-icon>attach_file</v-icon>
        </v-btn>
        <v-flex xs6>
          <v-text-field
            box
            label="Byte Position"
            color="info"
            type="number"
            v-model="bytePos"
            onkeydown="return event.keyCode == 69 || event.keyCode == 189 ? false : true"
            v-validate="'required|numeric|max:10'"
            :error-messages="errors.collect('Position')"
            data-vv-name="Position"
          ></v-text-field>
          <v-text-field
            box
            label="Bytes Request"
            color="info"
            type="number"
            v-model="byteReq"
            onkeydown="return event.keyCode == 69 || event.keyCode == 189 ? false : true"
            v-validate="'required|numeric|max:10'"
            :error-messages="errors.collect('Request')"
            data-vv-name="Request"
          ></v-text-field>
        </v-flex>
      </v-layout>

      <v-textarea outline label="Content of Bin File" v-model="content"></v-textarea>
      <v-btn color="info" raised @click="readFile">Read Bin File</v-btn>
    </v-form>
  </v-container>
</template>

<script>
export default {
  props: ["placeholder"],
  $_veeValidate: {
    validator: "new"
  },
  data() {
    return {
      fileName: "",
      fileUrl: "",
      fileFile: "",
      bytePos: "",
      byteReq: "",
      dictionary: {
        custom: {
          Position: {
            required: "Byte Position field is required"
          },
          Request: {
            required: "Byte Request field is required"
          }
        }
      },
      content: ""
    };
  },
  methods: {
    pickFile() {
      this.$refs.fileInput.click();
    },
    onFilePicked(e) {
      const files = e.target.files;
      // console.log(files);
      this.fileName = files[0].name;
      if (this.fileName.lastIndexOf(".") <= 0) {
        return alert("Please add a valid file!");
      }
      const fr = new FileReader();
      fr.addEventListener("load", () => {
        this.fileUrl = fr.result;
      });
      fr.readAsDataURL(files[0]);
      this.fileFile = files[0];
    },
    resetData() {
      this.fileName = "";
      this.fileFile = "";
      this.fileUrl = "";
    },
    readFile() {
      this.$validator.validateAll().then(result => {
        if (result) {
          axios
            .get(
              "/api/binfile?fileName=" +
                this.fileName +
                "&bytePos=" +
                this.bytePos +
                "&byteReq=" +
                this.byteReq
            )
            .then(res => {
              this.content = res.data
              this.$Progress.finish();
            })
            .catch(() => {
              Swal("Failed!", "Some error occurred", "warning");
              this.$Progress.fail();
            });
        }
      });
    }
  },
  created() {},
  mounted() {
    this.$validator.localize("en", this.dictionary);
  }
};
</script>