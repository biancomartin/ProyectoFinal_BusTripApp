package com.example.proyecto_final.ui.fragments

import androidx.lifecycle.MutableLiveData
import com.example.proyecto_final.BaseViewModel
import com.example.proyecto_final.ui.Event
import org.koin.core.KoinComponent

class SettingsViewModel :
    BaseViewModel(), KoinComponent {

    val settingsMutableLiveData = MutableLiveData<Event<Data>>()
    val liveData: MutableLiveData<Event<Data>>
        get() {
            return settingsMutableLiveData
        }


    data class Data(
        var status: Status,
        var data: Any? = null,
        var error: Exception? = null
    )

    enum class Status {
        LOADING,
        ERROR,
    }
}