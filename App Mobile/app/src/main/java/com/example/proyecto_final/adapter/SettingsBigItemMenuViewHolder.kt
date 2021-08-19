package com.example.proyecto_final.adapter

import android.view.View
import androidx.recyclerview.widget.RecyclerView
import com.example.proyecto_final.ui.MenuListItem
import kotlinx.android.synthetic.main.view_settings_menu_recycler_item.view.*


class SettingsBigItemMenuViewHolder(itemView: View) :
    RecyclerView.ViewHolder(itemView) {

    fun bind(item: MenuListItem) = with(itemView) {
        itemView.sub_menu.text = item.label
        setOnClickListener {
            item.action()
        }
    }

}
