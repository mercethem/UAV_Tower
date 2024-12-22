import pandas as pd
from sqlalchemy import create_engine
from dash import Dash, dcc, html, Input, Output
import plotly.graph_objects as go

# PostgreSQL veritabanı bağlantı bilgileri
DB_HOST = "localhost"
DB_PORT = "5432"
DB_NAME = "WeatherDataAnalysis"
DB_USER = "postgres"
DB_PASSWORD = "22"


# SQLAlchemy ile veritabanı bağlantısı ve veri çekme fonksiyonu
def fetch_weather_data_from_db():
    try:
        engine = create_engine(f"postgresql+psycopg2://{DB_USER}:{DB_PASSWORD}@{DB_HOST}:{DB_PORT}/{DB_NAME}")
        query = """
        SELECT id, description, temperature, feelslike, humidity, windspeed, 
               pressure, clouds, visibility, sunrise, sunset, city, recorddatetime
        FROM weather_data
        ORDER BY recorddatetime;
        """
        df = pd.read_sql(query, engine)
        df['recorddatetime'] = pd.to_datetime(df['recorddatetime'])
        df['sunrise'] = pd.to_datetime(df['sunrise'])
        df['sunset'] = pd.to_datetime(df['sunset'])
        return df
    except Exception as e:
        print(f"Veritabanı hatası: {e}")
        return None


# Dash uygulaması oluşturma
app = Dash(__name__)
df = fetch_weather_data_from_db()

if df is None or df.empty:
    print("Veritabanından veri çekilemedi veya veri bulunamadı.")
    exit()

# `recorddatetime` sütununu sadece tarih kısmına indirgemek
df['recorddate'] = df['recorddatetime'].dt.date

# Tema renk ayarları
light_theme = {
    'backgroundColor': '#d3e0ea',
    'textColor': '#2c3e50',
    'cardBackground': '#ecf0f1',
    'plotlyTemplate': 'plotly_white',
    'inputBackground': '#ffffff',
}

dark_theme = {
    'backgroundColor': '#2c3e50',
    'textColor': '#ecf0f1',
    'cardBackground': '#34495e',
    'plotlyTemplate': 'plotly_dark',
    'inputBackground': '#34495e',
}

# Uygulama düzeni (layout)
app.layout = html.Div(
    style={
        'fontFamily': 'Arial, sans-serif',
        'padding': '20px',
        'maxWidth': '1200px',
        'margin': 'auto',
        'borderRadius': '10px',
        'boxShadow': '0 4px 8px rgba(0, 0, 0, 0.1)'
    },
    children=[
        # Tema seçici (Açık/Koyu Mod)
        html.Div(
            children=[
                dcc.RadioItems(
                    id='theme-switch',
                    options=[
                        {'label': '🌞 Light Mode', 'value': 'light'},
                        {'label': '🌙 Dark Mode', 'value': 'dark'}
                    ],
                    value='light',
                    labelStyle={'display': 'inline-block', 'marginRight': '15px'}
                )
            ],
            style={'textAlign': 'right', 'marginBottom': '30px'}
        ),

        # Tema ayarlarını saklayan store
        dcc.Store(id='theme-store'),

        html.H1("Weather Data Analysis", style={'textAlign': 'center', 'fontSize': '2.5rem'}),

        html.Div([
            # Tarih aralığı seçici
            dcc.DatePickerRange(
                id='date-picker-range',
                min_date_allowed=df['recorddate'].min(),
                max_date_allowed=df['recorddate'].max(),
                start_date=df['recorddate'].min(),
                end_date=df['recorddate'].max(),
                style={'marginBottom': '20px'}
            ),

            # Ölçüm türü seçici
            dcc.Dropdown(
                id='metrics-dropdown',
                options=[{'label': col.capitalize(), 'value': col} for col in df.columns if
                         col not in ['id', 'recorddatetime', 'recorddate', 'city', 'description', 'sunrise', 'sunset']],
                value=['temperature', 'humidity'],
                multi=True,
                placeholder="Select Data Metrics",
                style={'marginBottom': '20px'}
            ),

            # Şehir seçici
            dcc.Dropdown(
                id='city-dropdown',
                options=[{'label': city, 'value': city} for city in df['city'].unique()],
                value=df['city'].unique().tolist(),
                multi=True,
                placeholder="Select Cities",
                style={'marginBottom': '20px'}
            ),
        ], style={'marginBottom': '40px'}),

        # Sunrise ve Sunset saatlerini gösteren bölüm
        html.Div(
            id='sun-times',
            style={
                'backgroundColor': '#ecf0f1',
                'padding': '15px',
                'borderRadius': '8px',
                'textAlign': 'center',
                'marginBottom': '40px',
                'color': '#2c3e50'
            }
        ),

        # Grafik alanı
        dcc.Graph(
            id='weather-graph',
            config={'displayModeBar': False},
            style={'borderRadius': '8px', 'boxShadow': '0 4px 8px rgba(0, 0, 0, 0.1)'}
        )
    ]
)


# Callback fonksiyonu: Tarih aralığı, ölçüm türü ve şehir değiştiğinde grafiği güncelle
@app.callback(
    [Output('weather-graph', 'figure'),
     Output('sun-times', 'children'),
     Output('theme-store', 'data')],
    [Input('date-picker-range', 'start_date'),
     Input('date-picker-range', 'end_date'),
     Input('metrics-dropdown', 'value'),
     Input('city-dropdown', 'value'),
     Input('theme-switch', 'value')]
)
def update_graph_and_suntimes(start_date, end_date, selected_metrics, selected_cities, theme_value):
    # Temayı seç
    theme = light_theme if theme_value == 'light' else dark_theme

    # Veriyi filtrele
    filtered_df = df[(df['recorddate'] >= pd.to_datetime(start_date).date()) &
                     (df['recorddate'] <= pd.to_datetime(end_date).date()) &
                     (df['city'].isin(selected_cities))]

    # Grafik oluşturma
    fig = go.Figure()

    # Dinamik olarak seçilen metrikler için grafik oluştur
    colors = {
        'temperature': 'firebrick',
        'humidity': 'blue',
        'windspeed': 'green',
        'pressure': 'orange',
        'feelslike': 'purple',
        'clouds': 'gray',
        'visibility': 'lightblue'
    }

    for metric in selected_metrics:
        fig.add_trace(go.Scatter(
            x=filtered_df['recorddatetime'],
            y=filtered_df[metric],
            mode='lines+markers',
            name=metric.capitalize(),
            line=dict(color=colors.get(metric, 'black'))
        ))

    fig.update_layout(
        title="",
        xaxis_title="Time",
        yaxis_title="Values",
        template=theme['plotlyTemplate'],
        hovermode='x unified',
        height=600,
        width=1000
    )

    # Seçilen tarihlerdeki en erken ve en geç sunrise/sunset saatlerini al
    if not filtered_df.empty:
        earliest_sunrise = filtered_df['sunrise'].min().time()
        latest_sunset = filtered_df['sunset'].max().time()
        sun_times_text = f"Sunrise: {earliest_sunrise} | Sunset: {latest_sunset}"
    else:
        sun_times_text = "Seçilen tarih aralığında Sunrise/Sunset verisi bulunamadı."

    return fig, sun_times_text, theme_value


if __name__ == '__main__':
    app.run_server(debug=True)
