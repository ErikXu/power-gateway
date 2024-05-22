<template>
  <div class="app-container">
    <h3>{{ $t('Request Log Detail') }}</h3>
    <el-row type="flex" style="margin-bottom:10px;" justify="end">
      <el-button type="primary" size="mini" @click="back">{{ $t('Back') }}</el-button>
    </el-row>
    <h4>{{ $t('Basic Info') }}</h4>
    <el-card class="box-card">
      <el-form label-width="30%" size="mini">
        <el-form-item :label="$t('Id')">
          <span>{{ detail.id }}</span>
        </el-form-item>
        <el-form-item :label="$t('Start Time')">
          <span>{{ detail.startTime | simpleFormat }}</span>
        </el-form-item>
        <el-form-item :label="$t('Url')">
          <el-tag v-if="detail.method === 'GET'" size="small" effect="dark" color="#61AFFE" style="border-color: #d9ecff;">{{ detail.method }}</el-tag>
          <el-tag v-if="detail.method === 'POST'" size="small" effect="dark" color="#49C990" style="border-color: #d9ecff;">{{ detail.method }}</el-tag>
          <el-tag v-if="detail.method === 'PUT'" size="small" effect="dark" color="#FCA130" style="border-color: #d9ecff;">{{ detail.method }}</el-tag>
          <el-tag v-if="detail.method === 'PATCH'" size="small" effect="dark" color="#50E3C2" style="border-color: #d9ecff;">{{ detail.method }}</el-tag>
          <el-tag v-if="detail.method === 'DELETE'" size="small" effect="dark" color="#F93E3E" style="border-color: #d9ecff;">{{ detail.method }}</el-tag>
          <span> {{ detail.url }} </span>
        </el-form-item>
        <el-form-item :label="$t('Upstream')">
          <span>{{ detail.upstream }}</span>
        </el-form-item>
        <el-form-item :label="$t('Latency')">
          <span>{{ detail.latency.toFixed(2) }}</span>
        </el-form-item>
        <el-form-item :label="$t('Client Ip')">
          <span>{{ detail.clientIp }}</span>
        </el-form-item>
        <el-form-item :label="$t('Status')">
          <span>{{ detail.status }}</span>
        </el-form-item>
      </el-form>
    </el-card>

    <h4>{{ $t('Request') }}</h4>
    <h5>{{ $t('Request Header') }}</h5>
    <el-table
      :data="detail.requestHeaders"
      border
      fit
      highlight-current-row
      style="margin-top:10px;"
    >
      <el-table-column label="#" align="center" width="55">
        <template slot-scope="scope">
          {{ scope.$index + 1 }}
        </template>
      </el-table-column>
      <el-table-column :label="$t('Key')" align="left" width="240">
        <template slot-scope="{row}">
          <span>{{ row.key }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Value')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.value }}</span>
        </template>
      </el-table-column>
    </el-table>
    <h5>{{ $t('Jwt Token') }}</h5>
    <el-table
      :data="detail.jwt || []"
      border
      fit
      highlight-current-row
      style="margin-top:10px;"
    >
      <el-table-column label="#" align="center" width="55">
        <template slot-scope="scope">
          {{ scope.$index + 1 }}
        </template>
      </el-table-column>
      <el-table-column :label="$t('Key')" align="left" width="240">
        <template slot-scope="{row}">
          <span>{{ row.key }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Value')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.value }}</span>
        </template>
      </el-table-column>
    </el-table>
    <h5>{{ $t('Query String') }}</h5>
    <el-table
      :data="detail.queryStrings"
      border
      fit
      highlight-current-row
      style="margin-top:10px;"
    >
      <el-table-column label="#" align="center" width="55">
        <template slot-scope="scope">
          {{ scope.$index + 1 }}
        </template>
      </el-table-column>
      <el-table-column :label="$t('Key')" align="left" width="240">
        <template slot-scope="{row}">
          <span>{{ row.key }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Value')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.value }}</span>
        </template>
      </el-table-column>
    </el-table>
    <h5>{{ $t('Request Body') }}</h5>
    <pre class="language-none"><code>{{ detail.requestBody || 'N/A' }}</code></pre>

    <h4>{{ $t('Response') }}</h4>
    <h5>{{ $t('Response Header') }}</h5>
    <el-table
      :data="detail.responseHeaders"
      border
      fit
      highlight-current-row
      style="margin-top:10px;"
    >
      <el-table-column label="#" align="center" width="55">
        <template slot-scope="scope">
          {{ scope.$index + 1 }}
        </template>
      </el-table-column>
      <el-table-column :label="$t('Key')" align="left" width="240">
        <template slot-scope="{row}">
          <span>{{ row.key }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Value')" align="left">
        <template slot-scope="{row}">
          <span>{{ row.value }}</span>
        </template>
      </el-table-column>
    </el-table>
    <h5>{{ $t('Response Body') }}</h5>
    <pre class="language-none"><code>{{ detail.responseBody || 'N/A' }}</code></pre>
  </div>
</template>

<script>
import { getApisixRequestLog } from '@/api/log'
import 'prismjs/themes/prism-okaidia.css'

export default {
  data() {
    return {
      detail: null
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    fetchData() {
      const id = this.$route.params.id
      getApisixRequestLog(id).then(response => {
        this.detail = response
      })
    },
    refresh() {
      this.fetchData()
    },
    back() {
      this.$router.push({ name: 'apisix-request-log' })
    }
  }
}
</script>

<i18n>
{
  "en": {
    "Request Log Detail": "Request Log Detail",
    "Back": "Back",
    "Basic Info": "Basic Info",
    "Id": "Id",
    "Start Time": "Start Time",
    "Url": "Url",
    "Upstream": "Upstream",
    "Latency": "Latency - ms",
    "Client Ip": "Client Ip",
    "Status": "Status",
    "Request": "Request",
    "Request Header": "Request Header",
    "Jwt Token": "Jwt Token",
    "Query String": "Query String",
    "Request Body": "Request Body",
    "Response": "Response",
    "Response Header": "Response Header",
    "Response Body": "Response Body",
    "Key": "Key",
    "Value": "Value",
    "None": "None"
  },
  "zh": {
    "Request Log Detail": "请求日志详情",
    "Back": "返回",
    "Basic Info": "基本信息",
    "Id": "标识",
    "Start Time": "请求时间",
    "Url": "请求地址",
    "Upstream": "上游",
    "Latency": "延时 - 毫秒",
    "Client Ip": "客户端 Ip",
    "Status": "状态码",
    "Request": "请求信息",
    "Request Header": "请求头",
    "Jwt Token": "Jwt 信息",
    "Query String": "请求参数",
    "Request Body": "请求体",
    "Response": "响应",
    "Response Header": "响应头",
    "Response Body": "响应体",
    "Key": "键",
    "Value": "值",
    "None": "无"
  }
}
</i18n>

