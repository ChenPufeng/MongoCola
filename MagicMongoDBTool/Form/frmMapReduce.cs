﻿using System;
using System.Collections.Generic;
using MagicMongoDBTool.Module;
using MongoDB.Bson;
using MongoDB.Driver;
namespace MagicMongoDBTool
{
    public partial class frmMapReduce : QLFUI.QLFForm
    {
        public frmMapReduce()
        {
            InitializeComponent();
        }
        private MongoCollection _mongocol = SystemManager.GetCurrentCollection();
        private void frmMapReduce_Load(object sender, EventArgs e)
        {
            cmbForMap.SelectedIndexChanged += new EventHandler(
                (x, y) => { txtMapJs.Text = MongoDBHelpler.LoadJavascript(cmbForMap.Text); }
            );
            cmbForReduce.SelectedIndexChanged += new EventHandler(
                (x, y) => { txtReduceJs.Text = MongoDBHelpler.LoadJavascript(cmbForReduce.Text); }
            );

            foreach (var item in SystemManager.GetJsNameList())
            {
                cmbForMap.Items.Add(item);
                cmbForReduce.Items.Add(item);
            }
        }

        private void cmdRun_Click(object sender, EventArgs e)
        {
            BsonJavaScript map = new BsonJavaScript(txtMapJs.Text);
            BsonJavaScript reduce = new BsonJavaScript(txtReduceJs.Text);
            //TODO:这里可能会超时，失去响应
            //需要设置SocketTimeOut
            MapReduceResult rtn = _mongocol.MapReduce(map, reduce);

            List<BsonDocument> result = new List<BsonDocument>();
            result.Add(rtn.Response);
            MongoDBHelpler.FillDataToTreeView("MapReduce Result", trvResult, result);
            trvResult.ExpandAll();
        }
        private void cmdSaveMapJs_Click(object sender, EventArgs e)
        {
            if (txtMapJs.Text != string.Empty)
            {
                String strJsName = Microsoft.VisualBasic.Interaction.InputBox("请输入Javascript名称：", "保存Javascript");
                MongoDBHelpler.SaveJavascript(strJsName, txtMapJs.Text);
            }
        }
        private void cmdSaveReduceJs_Click(object sender, EventArgs e)
        {
            if (this.txtReduceJs.Text != string.Empty)
            {
                String strJsName = Microsoft.VisualBasic.Interaction.InputBox("请输入Javascript名称：", "保存Javascript");
                MongoDBHelpler.SaveJavascript(strJsName, txtReduceJs.Text);
            }
        }



    }
}